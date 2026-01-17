using System.Windows;

namespace Ace.App.Views.Core
{
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        public void UpdateStatus(string status)
        {
            Dispatcher.Invoke(() => StatusText.Text = status);
        }
    }
}
