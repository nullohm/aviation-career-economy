using System.Windows.Controls;
using Ace.App.Infrastructure;
using Ace.App.Views.Core;

namespace Ace.App.Views.Menus
{
    public partial class DashboardMenuView : System.Windows.Controls.UserControl
    {
        public DashboardMenuView()
        {
            InitializeComponent();
            DashboardContent.Content = ServiceLocator.GetService<DashboardView>();
        }
    }
}
