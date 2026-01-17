using System.Windows.Controls;
using Ace.App.Infrastructure;
using Ace.App.Views.Core;

namespace Ace.App.Views.Menus
{
    public partial class HomeMenuView : System.Windows.Controls.UserControl
    {
        public HomeMenuView()
        {
            InitializeComponent();
            DashboardContent.Content = ServiceLocator.GetService<DashboardView>();
        }
    }
}
