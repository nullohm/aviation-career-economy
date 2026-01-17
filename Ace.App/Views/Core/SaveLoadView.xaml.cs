using System.Windows;
using System.Windows.Controls;

namespace Ace.App.Views.Core
{
    public partial class SaveLoadView : System.Windows.Controls.UserControl
    {
        public SaveLoadView()
        {
            InitializeComponent();
        }

        private void LoadSaves()
        {
            SavesList.ItemsSource = null;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Saving now happens automatically in the database!", "Auto-Save Active");
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Loading is no longer necessary - database is always up to date.", "Info");
        }
    }
}
