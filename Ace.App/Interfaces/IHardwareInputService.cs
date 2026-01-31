using System;

namespace Ace.App.Interfaces
{
    public enum HardwareDeviceRole : byte
    {
        Throttle = 0x02,
        Stick = 0x03
    }

    public class HardwareInputData
    {
        public HardwareDeviceRole Source { get; set; }
        public uint Sequence { get; set; }
        public ushort[] Axes { get; set; } = new ushort[8]; // 0-4095
        public uint Buttons { get; set; }
        public byte Hat { get; set; } = 0xFF;
    }

    public class HardwareDeviceStatus
    {
        public HardwareDeviceRole Device { get; set; }
        public bool IsConnected { get; set; }
        public uint UptimeMs { get; set; }
        public sbyte Rssi { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public interface IHardwareInputService
    {
        bool IsListening { get; }
        bool IsThrottleConnected { get; }
        bool IsStickConnected { get; }

        void StartListening(int port = 4269);
        void StopListening();

        event Action<HardwareInputData>? InputReceived;
        event Action<HardwareDeviceStatus>? DeviceStatusChanged;
    }
}
