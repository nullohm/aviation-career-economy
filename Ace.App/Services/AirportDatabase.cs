using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.IO;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Utilities;

namespace Ace.App.Services
{
    public class Airport
    {
        public string Icao { get; set; } = "";
        public string ICAO => Icao;
        public string Name { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int LongestRunwayFt { get; set; }
    }

    public class AirportDatabase : IAirportDatabase
    {
        private readonly ILoggingService _loggingService;
        private readonly ISettingsService _settingsService;
        private string? _dbPath;
        private readonly object _lock = new object();

        public bool IsAvailable => _dbPath != null;

        public AirportDatabase(ILoggingService loggingService, ISettingsService settingsService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public void Initialize(string? msfsPackagesPath = null)
        {
            _dbPath = LittleNavmapPathResolver.FindAirportDatabase(_settingsService);

            if (_dbPath != null)
            {
                _loggingService.Info($"Using Little Navmap airport database: {_dbPath}");
            }
            else
            {
                _loggingService.Warn("Little Navmap database not found - airport features will be unavailable");
            }
        }

        public Airport? GetAirport(string icao)
        {
            if (!IsAvailable)
            {
                _loggingService.Warn($"GetAirport: Little Navmap database not available, cannot look up {icao}");
                return null;
            }

            return GetAirportFromDb(icao.ToUpperInvariant());
        }

        private Airport? GetAirportFromDb(string icao)
        {
            try
            {
                lock (_lock)
                {
                    using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadOnly");
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT a.ident, a.name, a.lonx, a.laty,
                               COALESCE(a.longest_runway_length, 0) as longest_runway
                        FROM airport a
                        WHERE a.ident = @icao
                        LIMIT 1";

                    command.Parameters.AddWithValue("@icao", icao);

                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Airport
                        {
                            Icao = reader.GetString(0),
                            Name = reader.GetString(1),
                            Longitude = reader.GetDouble(2),
                            Latitude = reader.GetDouble(3),
                            LongestRunwayFt = reader.GetInt32(4)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Error getting airport {icao} from database", ex);
            }

            return null;
        }

        public AirportDetail? GetAirportDetail(string icao)
        {
            if (_dbPath == null)
            {
                _loggingService.Warn("GetAirportDetail: No Little Navmap database available");
                return null;
            }

            var icaoUpper = icao.ToUpperInvariant();

            try
            {
                lock (_lock)
                {
                    using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadOnly");
                    connection.Open();

                    var detail = new AirportDetail();

                    var airportCmd = connection.CreateCommand();
                    airportCmd.CommandText = @"
                        SELECT airport_id, ident, COALESCE(iata, ''), name, COALESCE(city, ''), COALESCE(country, ''),
                               lonx, laty, CAST(altitude AS INTEGER),
                               has_avgas, has_jetfuel, is_closed, is_military, is_addon,
                               COALESCE(tower_frequency, 0), COALESCE(atis_frequency, 0), COALESCE(unicom_frequency, 0)
                        FROM airport
                        WHERE ident = @icao
                        LIMIT 1";
                    airportCmd.Parameters.AddWithValue("@icao", icaoUpper);

                    int airportId = 0;
                    using (var reader = airportCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        airportId = reader.GetInt32(0);
                        detail.Icao = reader.GetString(1);
                        detail.Iata = reader.GetString(2);
                        detail.Name = reader.GetString(3);
                        detail.City = reader.GetString(4);
                        detail.Country = reader.GetString(5);
                        detail.Longitude = reader.GetDouble(6);
                        detail.Latitude = reader.GetDouble(7);
                        detail.Elevation = reader.GetInt32(8);
                        detail.HasAvgas = reader.GetInt32(9) == 1;
                        detail.HasJetfuel = reader.GetInt32(10) == 1;
                        detail.IsClosed = reader.GetInt32(11) == 1;
                        detail.IsMilitary = reader.GetInt32(12) == 1;
                        detail.IsAddon = reader.GetInt32(13) == 1;
                        detail.TowerFrequency = reader.GetInt32(14);
                        detail.AtisFrequency = reader.GetInt32(15);
                        detail.UnicomFrequency = reader.GetInt32(16);
                    }

                    var runwayCmd = connection.CreateCommand();
                    runwayCmd.CommandText = @"
                        SELECT CAST(r.length AS INTEGER), CAST(r.width AS INTEGER), COALESCE(r.surface, ''), r.heading,
                               re1.name, re2.name,
                               COALESCE(NULLIF(re1.ils_ident, ''), ils1.ident, ''),
                               COALESCE(NULLIF(re2.ils_ident, ''), ils2.ident, ''),
                               COALESCE(r.edge_light, ''), r.center_light IS NOT NULL
                        FROM runway r
                        JOIN runway_end re1 ON r.primary_end_id = re1.runway_end_id
                        JOIN runway_end re2 ON r.secondary_end_id = re2.runway_end_id
                        LEFT JOIN ils ils1 ON (ils1.loc_runway_end_id = re1.runway_end_id
                            OR (ils1.loc_runway_name = re1.name AND ils1.loc_airport_ident = @icao)
                            OR (ils1.name LIKE '%' || re1.name AND ABS(ils1.lonx - @lon) < 0.1 AND ABS(ils1.laty - @lat) < 0.1))
                        LEFT JOIN ils ils2 ON (ils2.loc_runway_end_id = re2.runway_end_id
                            OR (ils2.loc_runway_name = re2.name AND ils2.loc_airport_ident = @icao)
                            OR (ils2.name LIKE '%' || re2.name AND ABS(ils2.lonx - @lon) < 0.1 AND ABS(ils2.laty - @lat) < 0.1))
                        WHERE r.airport_id = @airportId
                        GROUP BY r.runway_id
                        ORDER BY r.length DESC";
                    runwayCmd.Parameters.AddWithValue("@airportId", airportId);
                    runwayCmd.Parameters.AddWithValue("@icao", icaoUpper);
                    runwayCmd.Parameters.AddWithValue("@lon", detail.Longitude);
                    runwayCmd.Parameters.AddWithValue("@lat", detail.Latitude);

                    using (var reader = runwayCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detail.Runways.Add(new RunwayInfo
                            {
                                LengthFt = reader.GetInt32(0),
                                WidthFt = reader.GetInt32(1),
                                Surface = reader.GetString(2),
                                Heading = reader.GetDouble(3),
                                Name1 = reader.GetString(4),
                                Name2 = reader.GetString(5),
                                Ils1Ident = reader.GetString(6),
                                Ils2Ident = reader.GetString(7),
                                LightingType = reader.GetString(8),
                                HasCenterLights = reader.GetInt32(9) == 1
                            });
                        }
                    }

                    var comCmd = connection.CreateCommand();
                    comCmd.CommandText = @"
                        SELECT type, frequency, COALESCE(name, '')
                        FROM com
                        WHERE airport_id = @airportId
                        ORDER BY
                            CASE type
                                WHEN 'ATIS' THEN 1
                                WHEN 'T' THEN 2
                                WHEN 'G' THEN 3
                                WHEN 'C' THEN 4
                                WHEN 'A' THEN 5
                                WHEN 'D' THEN 6
                                ELSE 7
                            END, frequency";
                    comCmd.Parameters.AddWithValue("@airportId", airportId);

                    using (var reader = comCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detail.Frequencies.Add(new ComFrequency
                            {
                                Type = reader.GetString(0),
                                Frequency = reader.GetInt32(1),
                                Name = reader.GetString(2)
                            });
                        }
                    }

                    var ilsCmd = connection.CreateCommand();
                    ilsCmd.CommandText = @"
                        SELECT ident, COALESCE(name, ''), frequency, COALESCE(loc_runway_name, ''), COALESCE(gs_pitch, 0), COALESCE(type, '')
                        FROM ils
                        WHERE loc_airport_ident = @icao
                           OR loc_runway_end_id IN (
                               SELECT re.runway_end_id FROM runway_end re
                               JOIN runway rw ON rw.primary_end_id = re.runway_end_id OR rw.secondary_end_id = re.runway_end_id
                               WHERE rw.airport_id = @airportId
                           )
                           OR (
                               ABS(lonx - @lon) < 0.1 AND ABS(laty - @lat) < 0.1
                               AND loc_airport_ident IS NULL OR loc_airport_ident = ''
                           )
                        ORDER BY COALESCE(loc_runway_name, name)";
                    ilsCmd.Parameters.AddWithValue("@icao", icaoUpper);
                    ilsCmd.Parameters.AddWithValue("@airportId", airportId);
                    ilsCmd.Parameters.AddWithValue("@lon", detail.Longitude);
                    ilsCmd.Parameters.AddWithValue("@lat", detail.Latitude);

                    using (var reader = ilsCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detail.IlsSystems.Add(new IlsInfo
                            {
                                Ident = reader.GetString(0),
                                Name = reader.GetString(1),
                                Frequency = reader.GetInt32(2),
                                RunwayName = reader.GetString(3),
                                GlideSlopePitch = reader.GetDouble(4),
                                Category = reader.GetString(5)
                            });
                        }
                    }

                    _loggingService.Debug($"GetAirportDetail: Loaded {detail.Icao} with {detail.Runways.Count} runways, {detail.Frequencies.Count} frequencies, {detail.IlsSystems.Count} ILS");
                    return detail;
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error($"Error getting airport detail for {icao}", ex);
                return null;
            }
        }

        public List<Airport> GetAllAirports()
        {
            if (!IsAvailable)
            {
                _loggingService.Warn("GetAllAirports: Little Navmap database not available");
                return new List<Airport>();
            }

            var airports = new List<Airport>();

            try
            {
                lock (_lock)
                {
                    using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadOnly");
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT ident, name, lonx, laty, COALESCE(longest_runway_length, 0)
                        FROM airport
                        WHERE ident IS NOT NULL
                        AND ident != ''
                        AND length(ident) = 4";

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        airports.Add(new Airport
                        {
                            Icao = reader.GetString(0),
                            Name = reader.GetString(1),
                            Longitude = reader.GetDouble(2),
                            Latitude = reader.GetDouble(3),
                            LongestRunwayFt = reader.GetInt32(4)
                        });
                    }
                }

                _loggingService.Info($"Loaded {airports.Count} airports from database");
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error getting all airports from database", ex);
            }

            return airports;
        }

        public double CalculateDistanceBetweenAirports(Airport airport1, Airport airport2)
        {
            return CalculateDistance(airport1.Latitude, airport1.Longitude, airport2.Latitude, airport2.Longitude);
        }

        public double CalculateBearing(Airport from, Airport to)
        {
            return CalculateBearing(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
        }

        public string FindNearestAirport(double latitude, double longitude, double maxDistanceNM = 10.0)
        {
            if (!IsAvailable)
            {
                _loggingService.Warn("FindNearestAirport: Little Navmap database not available");
                return GeneratePositionCode(latitude, longitude);
            }

            try
            {
                lock (_lock)
                {
                    using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadOnly");
                    connection.Open();

                    var latMin = latitude - (maxDistanceNM / 60.0);
                    var latMax = latitude + (maxDistanceNM / 60.0);
                    var lonMin = longitude - (maxDistanceNM / 60.0);
                    var lonMax = longitude + (maxDistanceNM / 60.0);

                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT ident, name, lonx, laty
                        FROM airport
                        WHERE laty BETWEEN @latMin AND @latMax
                        AND lonx BETWEEN @lonMin AND @lonMax
                        AND ident IS NOT NULL AND ident != ''
                        AND length(ident) = 4
                        LIMIT 100";

                    command.Parameters.AddWithValue("@latMin", latMin);
                    command.Parameters.AddWithValue("@latMax", latMax);
                    command.Parameters.AddWithValue("@lonMin", lonMin);
                    command.Parameters.AddWithValue("@lonMax", lonMax);

                    Airport? nearest = null;
                    double minDistance = double.MaxValue;

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var icao = reader.GetString(0);
                        var lon = reader.GetDouble(2);
                        var lat = reader.GetDouble(3);

                        var distance = CalculateDistance(latitude, longitude, lat, lon);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearest = new Airport { Icao = icao, Latitude = lat, Longitude = lon };
                        }
                    }

                    if (nearest != null && minDistance <= maxDistanceNM)
                    {
                        _loggingService.Info($"Nearest airport: {nearest.Icao} at {minDistance:F1} NM");
                        return nearest.Icao;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.Error("Error querying airport database", ex);
            }

            return GeneratePositionCode(latitude, longitude);
        }

        private string GeneratePositionCode(double latitude, double longitude)
        {
            string positionCode = $"POS{latitude:+00.00;-00.00}{longitude:+000.00;-000.00}";
            _loggingService.Info($"Generated position code: {positionCode}");
            return positionCode;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double r = 3440.065;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * r * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        private double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            double lat1Rad = lat1 * Math.PI / 180;
            double lat2Rad = lat2 * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double y = Math.Sin(dLon) * Math.Cos(lat2Rad);
            double x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                       Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(dLon);
            double bearing = Math.Atan2(y, x) * 180 / Math.PI;

            return (bearing + 360) % 360;
        }
    }
}
