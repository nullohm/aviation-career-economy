using System.Windows;

namespace Ace.App.Views.Dialogs
{
    public partial class InputDialog : Window
    {
        public string Message { get; }
        public string InputText => TxtInput.Text;

        public InputDialog(string title, string message)
        {
            Message = message;
            DataContext = this;
            InitializeComponent();
            Title = title;
            TxtInput.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
