using System.Windows;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Pilots
{
    public partial class PilotDetailView : Window
    {
        public PilotDetailView(PilotDetailViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.PurchaseCompleted += OnPurchaseCompleted;
            viewModel.ErrorOccurred += OnErrorOccurred;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnPurchaseCompleted(object? sender, string message)
        {
            var dialog = new MessageDialog("Success", message);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            var dialog = new MessageDialog("Error", error);
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}
