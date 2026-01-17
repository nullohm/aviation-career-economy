using System;
using BruTile;
using BruTile.Predefined;
using BruTile.Web;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;

namespace Ace.App.Helpers
{
    public enum MapLayerType
    {
        Street,
        Terrain,
        Satellite
    }

    public static class MapTileHelper
    {
        public static TileLayer CreateTileLayer(MapLayerType layerType = MapLayerType.Street)
        {
            var mapStyle = "Auto";
            var isDarkTheme = true;

            try
            {
                var settingsService = ServiceLocator.GetService<ISettingsService>();
                mapStyle = settingsService.CurrentSettings.MapStyle;

                var themeService = ServiceLocator.GetService<IThemeService>();
                isDarkTheme = themeService.CurrentTheme == "Dark";
            }
            catch (InvalidOperationException)
            {
                System.Diagnostics.Debug.WriteLine("MapTileHelper: Services not available during startup, using defaults");
            }

            bool useDarkMap = mapStyle switch
            {
                "Light" => false,
                "Dark" => true,
                _ => isDarkTheme
            };

            return layerType switch
            {
                MapLayerType.Terrain => CreateTerrainLayer(useDarkMap),
                MapLayerType.Satellite => CreateSatelliteLayer(),
                _ => CreateStreetLayer(useDarkMap)
            };
        }

        private static TileLayer CreateStreetLayer(bool useDarkMap)
        {
            if (useDarkMap)
            {
                var tileSource = new HttpTileSource(
                    new GlobalSphericalMercator(),
                    "https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png",
                    new[] { "a", "b", "c", "d" },
                    name: "CartoDB Dark Matter");
                return CreateSilentTileLayer(tileSource, "CartoDB Dark Matter");
            }
            else
            {
                var tileSource = new HttpTileSource(
                    new GlobalSphericalMercator(),
                    "https://cartodb-basemaps-{s}.global.ssl.fastly.net/rastertiles/voyager/{z}/{x}/{y}.png",
                    new[] { "a", "b", "c", "d" },
                    name: "CartoDB Voyager");
                return CreateSilentTileLayer(tileSource, "CartoDB Voyager");
            }
        }

        private static TileLayer CreateTerrainLayer(bool useDarkMap)
        {
            var tileSource = new HttpTileSource(
                new GlobalSphericalMercator(),
                "https://stamen-tiles-{s}.a.ssl.fastly.net/terrain/{z}/{x}/{y}.png",
                new[] { "a", "b", "c", "d" },
                name: "Stamen Terrain");
            return CreateSilentTileLayer(tileSource, "Stamen Terrain");
        }

        private static TileLayer CreateSatelliteLayer()
        {
            var tileSource = new HttpTileSource(
                new GlobalSphericalMercator(),
                "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}",
                name: "World Imagery");
            return CreateSilentTileLayer(tileSource, "World Imagery");
        }

        private static TileLayer CreateSilentTileLayer(ITileSource tileSource, string name)
        {
            var layer = new TileLayer(tileSource) { Name = name };
            return layer;
        }
    }
}
