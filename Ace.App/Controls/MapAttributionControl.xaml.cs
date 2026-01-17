using System.Windows.Controls;
using Ace.App.Helpers;

namespace Ace.App.Controls
{
    public partial class MapAttributionControl : UserControl
    {
        private MapLayerType _currentLayer = MapLayerType.Street;

        public MapAttributionControl()
        {
            InitializeComponent();
            UpdateAttribution();
        }

        public void SetMapLayer(MapLayerType layerType)
        {
            _currentLayer = layerType;
            UpdateAttribution();
        }

        private void UpdateAttribution()
        {
            TxtAttribution.Text = _currentLayer switch
            {
                MapLayerType.Street => "© OpenStreetMap contributors, © CartoDB",
                MapLayerType.Terrain => "© OpenStreetMap contributors, © OpenTopoMap",
                MapLayerType.Satellite => "© Esri, Maxar, Earthstar Geographics",
                _ => "© OpenStreetMap contributors"
            };
        }
    }
}
