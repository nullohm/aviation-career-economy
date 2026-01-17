using System.Windows;

namespace Ace.App.Views.Dialogs
{
    public partial class ConfirmDialog : Window
    {
        public bool Result { get; private set; }

        public ConfirmDialog(string title, string message, string yesText = "Yes", string noText = "No")
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            BtnYes.Content = yesText;
            BtnNo.Content = noText;
            Loaded += ConfirmDialog_Loaded;
        }

        private void ConfirmDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null) return;

            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            DialogResult = true;
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            DialogResult = false;
        }

        public static bool Show(string title, string message, Window? owner = null, string yesText = "Yes", string noText = "No")
        {
            var dialog = new ConfirmDialog(title, message, yesText, noText);
            if (owner != null)
                dialog.Owner = owner;
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}
