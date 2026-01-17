using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ace.App.Interfaces;

namespace Ace.App.Views.Finance
{
    public partial class DailyEarningsDetailWindow : Window
    {
        private readonly IDailyEarningsRepository _dailyEarningsRepository;

        public DailyEarningsDetailWindow(DateTime date, IDailyEarningsRepository dailyEarningsRepository)
        {
            _dailyEarningsRepository = dailyEarningsRepository;
            InitializeComponent();
            LoadDetails(date);
        }

        private void LoadDetails(DateTime date)
        {
            TxtDate.Text = date.ToString("dddd, dd MMMM yyyy");

            var details = _dailyEarningsRepository.GetDetailsByDate(date);

            DetailsGrid.ItemsSource = details;

            if (details.Any())
            {
                var totalHours = details.Sum(d => d.FlightHours);
                var totalDistance = details.Sum(d => d.DistanceNM);
                var totalRevenue = details.Sum(d => d.Revenue);
                var totalCosts = details.Sum(d => d.TotalCost);
                var totalProfit = details.Sum(d => d.Profit);

                var totalFuel = details.Sum(d => d.FuelCost);
                var totalMaint = details.Sum(d => d.MaintenanceCost);
                var totalIns = details.Sum(d => d.InsuranceCost);
                var totalCrew = details.Sum(d => d.CrewCost);
                var totalOther = details.Sum(d => d.OtherCosts);

                // Summary cards
                TxtTotalHours.Text = totalHours.ToString("N1");
                TxtTotalDistance.Text = totalDistance.ToString("N0");
                TxtTotalRevenue.Text = $"+€{totalRevenue:N0}";
                TxtTotalCosts.Text = $"-€{totalCosts:N0}";
                TxtTotalProfit.Text = totalProfit >= 0 ? $"+€{totalProfit:N0}" : $"-€{Math.Abs(totalProfit):N0}";

                // Cost distribution bar
                if (totalCosts > 0)
                {
                    var fuelPct = (double)(totalFuel / totalCosts) * 100;
                    var maintPct = (double)(totalMaint / totalCosts) * 100;
                    var insPct = (double)(totalIns / totalCosts) * 100;
                    var crewPct = (double)(totalCrew / totalCosts) * 100;
                    var otherPct = (double)(totalOther / totalCosts) * 100;

                    ColFuel.Width = new System.Windows.GridLength(fuelPct, System.Windows.GridUnitType.Star);
                    ColMaint.Width = new System.Windows.GridLength(maintPct, System.Windows.GridUnitType.Star);
                    ColIns.Width = new System.Windows.GridLength(insPct, System.Windows.GridUnitType.Star);
                    ColCrew.Width = new System.Windows.GridLength(crewPct, System.Windows.GridUnitType.Star);
                    ColOther.Width = new System.Windows.GridLength(otherPct, System.Windows.GridUnitType.Star);

                    TxtFuelPercent.Text = $"Fuel {fuelPct:N0}%";
                    TxtMaintPercent.Text = $"Maint {maintPct:N0}%";
                    TxtInsPercent.Text = $"Ins {insPct:N0}%";
                    TxtCrewPercent.Text = $"Crew {crewPct:N0}%";
                    TxtOtherPercent.Text = $"Other {otherPct:N0}%";
                }
            }
            else
            {
                TxtTotalHours.Text = "0";
                TxtTotalDistance.Text = "0";
                TxtTotalRevenue.Text = "€0";
                TxtTotalCosts.Text = "€0";
                TxtTotalProfit.Text = "€0";

                TxtFuelPercent.Text = "Fuel 0%";
                TxtMaintPercent.Text = "Maint 0%";
                TxtInsPercent.Text = "Ins 0%";
                TxtCrewPercent.Text = "Crew 0%";
                TxtOtherPercent.Text = "Other 0%";
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
