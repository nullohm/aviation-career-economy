using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ace.App.Interfaces;

namespace Ace.App.Views.Finance
{
    public partial class MonthlyEarningsReportWindow : Window
    {
        private readonly IDailyEarningsRepository _dailyEarningsRepository;

        public MonthlyEarningsReportWindow(int year, int month, IDailyEarningsRepository dailyEarningsRepository)
        {
            _dailyEarningsRepository = dailyEarningsRepository;
            InitializeComponent();
            LoadDetails(year, month);
        }

        private void LoadDetails(int year, int month)
        {
            TxtDate.Text = new DateTime(year, month, 1).ToString("MMMM yyyy");

            var details = _dailyEarningsRepository.GetDetailsByYearMonth(year, month);

            var aircraftSummaries = details
                .GroupBy(d => new { d.AircraftId, d.AircraftRegistration, d.AircraftType })
                .Select(g => new AircraftMonthlySummary
                {
                    AircraftRegistration = g.Key.AircraftRegistration,
                    AircraftType = g.Key.AircraftType,
                    Days = g.Select(d => d.Date.Date).Distinct().Count(),
                    FlightHours = g.Sum(d => d.FlightHours),
                    DistanceNM = g.Sum(d => d.DistanceNM),
                    Revenue = g.Sum(d => d.Revenue),
                    FuelCost = g.Sum(d => d.FuelCost),
                    MaintenanceCost = g.Sum(d => d.MaintenanceCost),
                    InsuranceCost = g.Sum(d => d.InsuranceCost),
                    DepreciationCost = g.Sum(d => d.DepreciationCost),
                    CrewCost = g.Sum(d => d.CrewCost),
                    FBOCost = g.Sum(d => d.FBOCost)
                })
                .OrderByDescending(a => a.Profit)
                .ToList();

            DetailsGrid.ItemsSource = aircraftSummaries;

            var totalDays = details.Select(d => d.Date.Date).Distinct().Count();
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

            TxtDays.Text = totalDays.ToString();
            TxtTotalHours.Text = totalHours.ToString("N1");
            TxtTotalDistance.Text = totalDistance.ToString("N0");
            TxtTotalRevenue.Text = $"+€{totalRevenue:N0}";
            TxtTotalCosts.Text = $"-€{totalCosts:N0}";
            TxtTotalProfit.Text = totalProfit >= 0 ? $"+€{totalProfit:N0}" : $"-€{Math.Abs(totalProfit):N0}";

            if (totalCosts > 0)
            {
                var fuelPct = (double)(totalFuel / totalCosts) * 100;
                var maintPct = (double)(totalMaint / totalCosts) * 100;
                var insPct = (double)(totalIns / totalCosts) * 100;
                var crewPct = (double)(totalCrew / totalCosts) * 100;
                var otherPct = (double)(totalOther / totalCosts) * 100;

                ColFuel.Width = new GridLength(fuelPct, GridUnitType.Star);
                ColMaint.Width = new GridLength(maintPct, GridUnitType.Star);
                ColIns.Width = new GridLength(insPct, GridUnitType.Star);
                ColCrew.Width = new GridLength(crewPct, GridUnitType.Star);
                ColOther.Width = new GridLength(otherPct, GridUnitType.Star);

                TxtFuelPercent.Text = $"Fuel {fuelPct:N0}%";
                TxtMaintPercent.Text = $"Maint {maintPct:N0}%";
                TxtInsPercent.Text = $"Ins {insPct:N0}%";
                TxtCrewPercent.Text = $"Crew {crewPct:N0}%";
                TxtOtherPercent.Text = $"Other {otherPct:N0}%";
            }
            else
            {
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

    public class AircraftMonthlySummary
    {
        public string AircraftRegistration { get; set; } = string.Empty;
        public string AircraftType { get; set; } = string.Empty;
        public int Days { get; set; }
        public double FlightHours { get; set; }
        public double DistanceNM { get; set; }
        public decimal Revenue { get; set; }
        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal InsuranceCost { get; set; }
        public decimal DepreciationCost { get; set; }
        public decimal CrewCost { get; set; }
        public decimal FBOCost { get; set; }

        public decimal TotalCost => FuelCost + MaintenanceCost + InsuranceCost + DepreciationCost + CrewCost + FBOCost;
        public decimal Profit => Revenue - TotalCost;
        public decimal AvgProfitPerDay => Days > 0 ? Profit / Days : 0;
        public bool IsProfitNegative => Profit < 0;
    }
}
