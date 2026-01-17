using System.Windows.Controls;
using Ace.App.Infrastructure;
using Ace.App.Views.Core;

namespace Ace.App.Views.Menus
{
    public partial class SettingsMenuView : System.Windows.Controls.UserControl
    {
        public SettingsMenuView()
        {
            InitializeComponent();
            SettingsContent.Content = ServiceLocator.GetService<SettingsView>();
        }
    }
}
