using System.Windows;
using System.Windows.Input;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Aircraft
{
    public partial class MaintenanceScheduleView : Window
    {
        private readonly MaintenanceScheduleViewModel _viewModel;

        public MaintenanceScheduleView(MaintenanceScheduleViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _viewModel.OperationCompleted += OnOperationCompleted;
            _viewModel.ErrorOccurred += OnErrorOccurred;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnCheckClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is MaintenanceCheckViewModel check)
            {
                _viewModel.SelectedCheck = check;
            }
        }

        private void OnScheduleClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is MaintenanceCheckViewModel check)
            {
                _viewModel.SelectedCheck = check;
                if (_viewModel.ScheduleCheckCommand.CanExecute(null))
                {
                    _viewModel.ScheduleCheckCommand.Execute(null);
                }
            }
        }

        private void OnOperationCompleted(object? sender, string message)
        {
            var dialog = new MessageDialog("Maintenance", message);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void OnErrorOccurred(object? sender, string message)
        {
            var dialog = new MessageDialog("Error", message);
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}
