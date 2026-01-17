using System;
using System.Windows;
using System.Windows.Controls;
using Ace.App.Views.FBO;

namespace Ace.App.Views.Menus
{
    public partial class FBOMenuView : UserControl
    {
        private readonly FBOView _fboView;

        public FBOMenuView(FBOView fboView)
        {
            InitializeComponent();
            _fboView = fboView ?? throw new ArgumentNullException(nameof(fboView));

            Loaded += FBOMenuView_Loaded;
        }

        private void FBOMenuView_Loaded(object sender, RoutedEventArgs e)
        {
            FBOContent.Content = _fboView;
        }
    }
}
