using System.Windows;

namespace Ace.App.Views.Dialogs
{
    public partial class InfoDialog : Window
    {
        public InfoDialog(string title, string message, string okText = "OK")
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;
            BtnOk.Content = okText;
            Loaded += InfoDialog_Loaded;
        }

        private void InfoDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null) return;

            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public static void Show(string title, string message, Window? owner = null)
        {
            var dialog = new InfoDialog(title, message);
            if (owner != null)
                dialog.Owner = owner;
            dialog.ShowDialog();
        }
    }
}
