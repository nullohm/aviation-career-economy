using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Ace.App.Interfaces;

namespace Ace.App.Services
{
    public class HardwareInputService : IHardwareInputService, IDisposable
    {
        // Must match acepit_protocol.h packed structs exactly
        private const byte MSG_INPUT_DATA = 0x10;
        private const byte MSG_HEARTBEAT = 0x20;
        private const int INPUT_PACKET_SIZE = 28;
        private const int HEARTBEAT_PACKET_SIZE = 9;
        private const int CONNECTION_TIMEOUT_MS = 3000;

        private readonly ILoggingService _logger;
        private UdpClient? _udpClient;
        private Thread? _listenerThread;
        private CancellationTokenSource? _cts;

        private DateTime _throttleLastSeen;
        private DateTime _stickLastSeen;
        private bool _throttleConnected;
        private bool _stickConnected;
        private Timer? _connectionCheckTimer;

        public bool IsListening => _udpClient != null;
        public bool IsThrottleConnected => _throttleConnected;
        public bool IsStickConnected => _stickConnected;

        public event Action<HardwareInputData>? InputReceived;
        public event Action<HardwareDeviceStatus>? DeviceStatusChanged;

        public HardwareInputService(ILoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void StartListening(int port = 4269)
        {
            if (_udpClient != null) return;

            try
            {
                _udpClient = new UdpClient(port);
                _cts = new CancellationTokenSource();

                _listenerThread = new Thread(ListenLoop)
                {
                    IsBackground = true,
                    Name = "AcepitUdpListener"
                };
                _listenerThread.Start();

                _connectionCheckTimer = new Timer(CheckConnections, null, 1000, 1000);

                _logger.Info($"[HW] Listening for ACEPIT devices on UDP port {port}");
            }
            catch (Exception ex)
            {
                _logger.Error($"[HW] Failed to start UDP listener: {ex.Message}");
                _udpClient?.Dispose();
                _udpClient = null;
            }
        }

        public void StopListening()
        {
            _cts?.Cancel();
            _connectionCheckTimer?.Dispose();
            _connectionCheckTimer = null;
            _udpClient?.Close();
            _udpClient?.Dispose();
            _udpClient = null;
            _listenerThread = null;

            SetDeviceConnected(HardwareDeviceRole.Throttle, false);
            SetDeviceConnected(HardwareDeviceRole.Stick, false);

            _logger.Info("[HW] UDP listener stopped");
        }

        private void ListenLoop()
        {
            var endpoint = new IPEndPoint(IPAddress.Any, 0);

            while (_cts != null && !_cts.IsCancellationRequested)
            {
                try
                {
                    if (_udpClient == null) break;
                    byte[] data = _udpClient.Receive(ref endpoint);
                    ProcessPacket(data);
                }
                catch (SocketException) when (_cts?.IsCancellationRequested == true)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Debug($"[HW] UDP receive error: {ex.Message}");
                }
            }
        }

        private void ProcessPacket(byte[] data)
        {
            if (data.Length < 3) return;

            byte messageType = data[1];

            switch (messageType)
            {
                case MSG_INPUT_DATA when data.Length >= INPUT_PACKET_SIZE:
                    ProcessInputPacket(data);
                    break;

                case MSG_HEARTBEAT when data.Length >= HEARTBEAT_PACKET_SIZE:
                    ProcessHeartbeat(data);
                    break;
            }
        }

        private void ProcessInputPacket(byte[] data)
        {
            // Parse packed struct: version(1) + type(1) + source(1) + sequence(4) + axes(16) + buttons(4) + hat(1)
            var input = new HardwareInputData
            {
                Source = (HardwareDeviceRole)data[2],
                Sequence = BitConverter.ToUInt32(data, 3)
            };

            for (int i = 0; i < 8; i++)
            {
                input.Axes[i] = BitConverter.ToUInt16(data, 7 + i * 2);
            }

            input.Buttons = BitConverter.ToUInt32(data, 23);
            input.Hat = data[27];

            UpdateLastSeen(input.Source);
            InputReceived?.Invoke(input);
        }

        private void ProcessHeartbeat(byte[] data)
        {
            // Parse packed struct: version(1) + type(1) + source(1) + uptime(4) + battery(1) + rssi(1)
            var role = (HardwareDeviceRole)data[2];
            uint uptime = BitConverter.ToUInt32(data, 3);
            sbyte rssi = (sbyte)data[8];

            UpdateLastSeen(role);

            DeviceStatusChanged?.Invoke(new HardwareDeviceStatus
            {
                Device = role,
                IsConnected = true,
                UptimeMs = uptime,
                Rssi = rssi,
                LastSeen = DateTime.UtcNow
            });
        }

        private void UpdateLastSeen(HardwareDeviceRole role)
        {
            var now = DateTime.UtcNow;
            switch (role)
            {
                case HardwareDeviceRole.Throttle:
                    _throttleLastSeen = now;
                    SetDeviceConnected(HardwareDeviceRole.Throttle, true);
                    break;
                case HardwareDeviceRole.Stick:
                    _stickLastSeen = now;
                    SetDeviceConnected(HardwareDeviceRole.Stick, true);
                    break;
            }
        }

        private void CheckConnections(object? state)
        {
            var now = DateTime.UtcNow;
            var timeout = TimeSpan.FromMilliseconds(CONNECTION_TIMEOUT_MS);

            if (_throttleConnected && (now - _throttleLastSeen) > timeout)
                SetDeviceConnected(HardwareDeviceRole.Throttle, false);

            if (_stickConnected && (now - _stickLastSeen) > timeout)
                SetDeviceConnected(HardwareDeviceRole.Stick, false);
        }

        private void SetDeviceConnected(HardwareDeviceRole role, bool connected)
        {
            bool changed = false;
            switch (role)
            {
                case HardwareDeviceRole.Throttle:
                    if (_throttleConnected != connected)
                    {
                        _throttleConnected = connected;
                        changed = true;
                    }
                    break;
                case HardwareDeviceRole.Stick:
                    if (_stickConnected != connected)
                    {
                        _stickConnected = connected;
                        changed = true;
                    }
                    break;
            }

            if (changed)
            {
                string name = role == HardwareDeviceRole.Throttle ? "Throttle" : "Stick";
                _logger.Info($"[HW] {name} {(connected ? "connected" : "disconnected")}");

                DeviceStatusChanged?.Invoke(new HardwareDeviceStatus
                {
                    Device = role,
                    IsConnected = connected,
                    LastSeen = connected ? DateTime.UtcNow : default
                });
            }
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}
