using System;
using System.Windows;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class SelectFBOTypeDialog : Window
    {
        public FBOType SelectedType { get; private set; } = FBOType.Local;
        public bool Confirmed { get; private set; }

        public SelectFBOTypeDialog(string icao, string airportName, ISettingsService settingsService)
        {
            if (settingsService == null) throw new ArgumentNullException(nameof(settingsService));

            InitializeComponent();

            TitleText.Text = $"Rent FBO at {icao}";
            TxtAirportInfo.Text = $"{icao} - {airportName}";

            var settings = settingsService.CurrentSettings;

            TxtLocalInfo.Text = $"{settings.FBORentLocal:N0} €/mo • {settings.RouteSlotLimitLocal} route slots";
            TxtRegionalInfo.Text = $"{settings.FBORentRegional:N0} €/mo • {settings.RouteSlotLimitRegional} route slots";
            TxtInternationalInfo.Text = $"{settings.FBORentInternational:N0} €/mo • {settings.RouteSlotLimitInternational} route slots";

            Loaded += SelectFBOTypeDialog_Loaded;
        }

        private void SelectFBOTypeDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null) return;

            Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
        }

        private void BtnRent_Click(object sender, RoutedEventArgs e)
        {
            if (RbLocal.IsChecked == true)
                SelectedType = FBOType.Local;
            else if (RbRegional.IsChecked == true)
                SelectedType = FBOType.Regional;
            else if (RbInternational.IsChecked == true)
                SelectedType = FBOType.International;

            Confirmed = true;
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
        }
    }
}
