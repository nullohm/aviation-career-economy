using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ace.App.Interfaces;

namespace Ace.App.Views.Finance
{
    public partial class MonthlyBillingDetailWindow : Window
    {
        private readonly IMonthlyBillingRepository _monthlyBillingRepository;

        public MonthlyBillingDetailWindow(DateTime billingDate, IMonthlyBillingRepository monthlyBillingRepository)
        {
            _monthlyBillingRepository = monthlyBillingRepository;
            InitializeComponent();
            LoadDetails(billingDate);
        }

        private void LoadDetails(DateTime billingDate)
        {
            TxtDate.Text = billingDate.ToString("MMMM yyyy");

            var details = _monthlyBillingRepository.GetDetailsByMonth(billingDate.Year, billingDate.Month);

            DetailsGrid.ItemsSource = details;

            if (details.Any())
            {
                var fboCosts = details.Where(d => d.Category == "FBO").Sum(d => d.Amount);
                var pilotCosts = details.Where(d => d.Category == "Pilot").Sum(d => d.Amount);
                var totalCosts = details.Sum(d => d.Amount);

                TxtFBOCosts.Text = $"-€{fboCosts:N0}";
                TxtPilotCosts.Text = $"-€{pilotCosts:N0}";
                TxtTotalCosts.Text = $"-€{totalCosts:N0}";
            }
            else
            {
                TxtFBOCosts.Text = "€0";
                TxtPilotCosts.Text = "€0";
                TxtTotalCosts.Text = "€0";
            }
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
