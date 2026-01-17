using System;
using System.Windows.Controls;

namespace Ace.App.Interfaces
{


    public interface INavigationService
    {


        void NavigateTo<TView>() where TView : UserControl;


        void NavigateTo(Type viewType);
    }
}
