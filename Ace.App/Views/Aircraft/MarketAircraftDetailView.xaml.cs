using System.Windows;
using Ace.App.ViewModels;

namespace Ace.App.Views.Aircraft
{
    public partial class MarketAircraftDetailView : Window
    {
        public MarketAircraftDetailView(MarketAircraftDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
