using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ace.App.Controls;
using Ace.App.Helpers;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using Ace.App.ViewModels;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.ComponentModel;

namespace Ace.App.Views.Finance
{
    public partial class StatisticsView : UserControl, INotifyPropertyChanged
    {
        private List<FlightRecord> _allFlights = new();
        private List<FlightRecord> _filteredFlights = new();
        private readonly IPersistenceService _persistenceService;
        private readonly IFinanceService _financeService;
        private readonly ISettingsService _settingsService;
        private readonly FlightMapViewModel _flightMapViewModel;
        private readonly IDailyEarningsRepository _dailyEarningsRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPilotRepository _pilotRepository;

        private PlotModel? _revenueExpenseChartModel;
        public PlotModel? RevenueExpenseChartModel
        {
            get => _revenueExpenseChartModel;
            set
            {
                _revenueExpenseChartModel = value;
                OnPropertyChanged(nameof(RevenueExpenseChartModel));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StatisticsView(
            IPersistenceService persistenceService,
            IFinanceService financeService,
            ISettingsService settingsService,
            FlightMapViewModel flightMapViewModel,
            IDailyEarningsRepository dailyEarningsRepository,
            IAircraftRepository aircraftRepository,
            ITransactionRepository transactionRepository,
            IPilotRepository pilotRepository)
        {
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _flightMapViewModel = flightMapViewModel ?? throw new ArgumentNullException(nameof(flightMapViewModel));
            _dailyEarningsRepository = dailyEarningsRepository ?? throw new ArgumentNullException(nameof(dailyEarningsRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));

            InitializeComponent();
            DataContext = this;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            if (Application.Current.MainWindow is MainWindow mw)
            {
                mw.SimConnectService.FlightRecorded += OnFlightRecorded;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAllData();
            InitializeMap();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _flightMapViewModel.Cleanup();
        }

        private void InitializeMap()
        {
            if (_flightMapViewModel.Map != null)
            {
                MapControl.Map = _flightMapViewModel.Map;
                _flightMapViewModel.LoadFlightData();
                UpdateMapStatus();

                FlightMapScaleControl.AttachToMap(MapControl.Map);

                var savedLayer = Enum.TryParse<MapLayerType>(_settingsService.CurrentSettings.MapLayer, out var layer)
                    ? layer
                    : MapLayerType.Street;
                UpdateMapLayerButtons(savedLayer);
                UpdateLegendVisibility(savedLayer);
                InitializeFlightMapLegend();

                // Initialize legend expanded state from settings
                LegendPanel.IsExpanded = _settingsService.CurrentSettings.MapLegendExpanded;
                LegendPanel.ExpandedStateChanged += OnLegendExpandedStateChanged;
            }
        }

        private void OnLegendExpandedStateChanged(bool isExpanded)
        {
            _settingsService.CurrentSettings.MapLegendExpanded = isExpanded;
            _settingsService.Save();
        }

        private void InitializeFlightMapLegend()
        {
            var runwayEntries = new List<MapLegendEntry>
            {
                new("< 2,000 ft", new SolidColorBrush(Color.FromRgb(144, 202, 249))),
                new("< 4,000 ft", new SolidColorBrush(Color.FromRgb(100, 181, 246))),
                new("< 6,000 ft", new SolidColorBrush(Color.FromRgb(66, 165, 245))),
                new("< 8,000 ft", new SolidColorBrush(Color.FromRgb(33, 150, 243))),
                new("8,000+ ft", new SolidColorBrush(Color.FromRgb(156, 39, 176)))
            };
            LegendPanel.SetRunwayLegend(runwayEntries);
        }

        private void UpdateMapStatus()
        {
            var flightCount = _allFlights.Count;
            TxtMapStatus.Text = $"{flightCount} flights displayed";
        }

        private void BtnRefreshMap_Click(object sender, RoutedEventArgs e)
        {
            _flightMapViewModel.LoadFlightData();
            UpdateMapStatus();
        }

        private void BtnCenterAircraft_Click(object sender, RoutedEventArgs e)
        {
            _flightMapViewModel.CenterOnAircraft();
        }

        private void LoadAllData()
        {
            LoadEarningsStats();
            LoadFlights();
            LoadFleetStats();
            LoadFinanceStats();
            LoadPilotStats();
        }

        private void OnFlightRecorded()
        {
            Dispatcher.Invoke(LoadAllData);
        }

        private void LoadEarningsStats()
        {
            var earnings = _dailyEarningsRepository.GetAllDetails();

            if (earnings.Count == 0)
            {
                TxtEarningsTotalProfit.Text = "€0";
                TxtEarningsTotalRevenue.Text = "€0";
                TxtEarningsTotalCosts.Text = "€0";
                TxtEarningsFlightHours.Text = "0h";
                TxtEarningsDistance.Text = "0 NM";
                TxtEarningsProfitMargin.Text = "0%";
                DailyProfitChart.Model = CreateEmptyPlotModel();
                TopAircraftGrid.ItemsSource = null;
                return;
            }

            var totalRevenue = earnings.Sum(e => e.Revenue);
            var totalCosts = earnings.Sum(e => e.TotalCost);
            var totalProfit = totalRevenue - totalCosts;
            var totalHours = earnings.Sum(e => e.FlightHours);
            var totalDistance = earnings.Sum(e => e.DistanceNM);
            var profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0;

            TxtEarningsTotalProfit.Text = $"€{totalProfit:N0}";
            TxtEarningsTotalRevenue.Text = $"€{totalRevenue:N0}";
            TxtEarningsTotalCosts.Text = $"€{totalCosts:N0}";
            TxtEarningsFlightHours.Text = $"{totalHours:N1}h";
            TxtEarningsDistance.Text = $"{totalDistance:N0} NM";
            TxtEarningsProfitMargin.Text = $"{profitMargin:F1}%";

            LoadDailyProfitChart(earnings);
            LoadTopAircraftGrid(earnings);
        }

        private void LoadDailyProfitChart(List<DailyEarningsDetail> earnings)
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.Gray
            };

            var last30Days = DateTime.Now.AddDays(-30);
            var dailyData = earnings
                .Where(e => e.Date >= last30Days)
                .GroupBy(e => e.Date.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key,
                    Profit = g.Sum(e => e.Profit)
                })
                .ToList();

            if (dailyData.Count == 0)
            {
                DailyProfitChart.Model = plotModel;
                return;
            }

            var barSeries = new BarSeries
            {
                StrokeThickness = 0,
                TrackerFormatString = "{Category}: €{Value:N0}"
            };

            foreach (var day in dailyData)
            {
                var color = day.Profit >= 0
                    ? OxyColor.FromRgb(76, 175, 80)
                    : OxyColor.FromRgb(244, 67, 54);
                barSeries.Items.Add(new BarItem { Value = (double)day.Profit, Color = color });
            }

            plotModel.Series.Add(barSeries);

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                GapWidth = 0.2
            };

            foreach (var day in dailyData)
            {
                categoryAxis.Labels.Add(day.Date.ToString("dd.MM"));
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(50, 50, 50),
                LabelFormatter = value => FormatCurrency(value)
            };

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            DailyProfitChart.Model = plotModel;
        }

        private void LoadTopAircraftGrid(List<DailyEarningsDetail> earnings)
        {
            var aircraftStats = earnings
                .GroupBy(e => new { e.AircraftId, e.AircraftRegistration, e.AircraftType })
                .Select(g => new AircraftPerformanceViewModel
                {
                    Registration = g.Key.AircraftRegistration,
                    Type = g.Key.AircraftType,
                    DaysActive = g.Select(e => e.Date.Date).Distinct().Count(),
                    TotalHours = g.Sum(e => e.FlightHours),
                    TotalDistance = g.Sum(e => e.DistanceNM),
                    TotalRevenue = g.Sum(e => e.Revenue),
                    TotalCosts = g.Sum(e => e.TotalCost),
                    TotalProfit = g.Sum(e => e.Profit)
                })
                .OrderByDescending(a => a.TotalProfit)
                .ToList();

            TopAircraftGrid.ItemsSource = aircraftStats;
        }

        private static PlotModel CreateEmptyPlotModel()
        {
            return new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.Gray
            };
        }

        private void LoadFlights()
        {
            var flights = _persistenceService.LoadFlightRecords();
            if (flights != null)
            {
                _allFlights = flights.OrderByDescending(f => f.Date).ToList();
                PopulateAircraftFilter();
                ApplyFilter();
                LoadFlightStats();
                LoadMonthlyFlightsChart();
            }
        }

        private void LoadFlightStats()
        {
            var totalFlights = _allFlights.Count;
            var totalDistance = _allFlights.Sum(f => f.DistanceNM);
            var totalDuration = TimeSpan.FromTicks(_allFlights.Sum(f => f.Duration.Ticks));
            var avgLandingRate = _allFlights.Count > 0
                ? _allFlights.Average(f => Math.Abs(f.LandingRate))
                : 0;

            TxtTotalFlights.Text = totalFlights.ToString();
            TxtTotalFlightDistance.Text = $"{totalDistance:N0} NM";
            TxtTotalFlightDuration.Text = $"{(int)totalDuration.TotalHours}h {totalDuration.Minutes}m";
            TxtAvgLandingRate.Text = $"-{avgLandingRate:F0} fpm";
        }

        private void LoadMonthlyFlightsChart()
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.Gray
            };

            if (_allFlights.Count == 0)
            {
                MonthlyFlightsChart.Model = plotModel;
                return;
            }

            var monthlyData = _allFlights
                .GroupBy(f => new { f.Date.Year, f.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count(),
                    Distance = g.Sum(f => f.DistanceNM)
                })
                .TakeLast(12)
                .ToList();

            var flightsSeries = new BarSeries
            {
                FillColor = OxyColor.FromRgb(33, 150, 243),
                StrokeThickness = 0
            };

            foreach (var month in monthlyData)
            {
                flightsSeries.Items.Add(new BarItem { Value = month.Count });
            }

            plotModel.Series.Add(flightsSeries);

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                GapWidth = 0.2
            };

            foreach (var month in monthlyData)
            {
                categoryAxis.Labels.Add(month.Date.ToString("MMM yy"));
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(50, 50, 50),
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            MonthlyFlightsChart.Model = plotModel;
        }

        private void PopulateAircraftFilter()
        {
            var selectedAircraft = (CmbAircraftFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();

            CmbAircraftFilter.Items.Clear();
            CmbAircraftFilter.Items.Add(new ComboBoxItem { Content = "All Aircraft", IsSelected = true });

            var aircraftList = _allFlights
                .Where(f => !string.IsNullOrEmpty(f.Aircraft))
                .Select(f => f.Aircraft)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            foreach (var aircraft in aircraftList)
            {
                var item = new ComboBoxItem { Content = aircraft };
                if (aircraft == selectedAircraft)
                {
                    item.IsSelected = true;
                    ((ComboBoxItem)CmbAircraftFilter.Items[0]).IsSelected = false;
                }
                CmbAircraftFilter.Items.Add(item);
            }
        }

        private void ApplyFilter()
        {
            var selectedAircraft = (CmbAircraftFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var searchText = TxtFilter.Text?.Trim().ToUpperInvariant() ?? "";

            _filteredFlights = _allFlights;

            if (!string.IsNullOrEmpty(selectedAircraft) && selectedAircraft != "All Aircraft")
            {
                _filteredFlights = _filteredFlights.Where(f => f.Aircraft == selectedAircraft).ToList();
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                _filteredFlights = _filteredFlights.Where(f =>
                    (f.Departure?.ToUpperInvariant().Contains(searchText) == true) ||
                    (f.Arrival?.ToUpperInvariant().Contains(searchText) == true) ||
                    (f.Aircraft?.ToUpperInvariant().Contains(searchText) == true) ||
                    (f.AircraftTitle?.ToUpperInvariant().Contains(searchText) == true)
                ).ToList();
            }

            FlightsGrid.ItemsSource = _filteredFlights;
            UpdateCountText();
            UpdateFilterHint();
        }

        private void UpdateCountText()
        {
            if (_filteredFlights.Count == _allFlights.Count)
            {
                FlightCountText.Text = $"{_allFlights.Count} flights";
            }
            else
            {
                FlightCountText.Text = $"{_filteredFlights.Count} of {_allFlights.Count} flights";
            }
        }

        private void UpdateFilterHint()
        {
            TxtFilterHint.Visibility = string.IsNullOrEmpty(TxtFilter.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void CmbAircraftFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilter();
            }
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void LoadFleetStats()
        {
            var aircraft = _aircraftRepository.GetAllAircraft();

            var totalCount = aircraft.Count;
            var totalValue = aircraft.Sum(a => a.CurrentValue);
            var totalHours = aircraft.Sum(a => a.TotalFlightHours);
            var maintenanceCount = aircraft.Count(a => a.Status == AircraftStatus.Maintenance);
            var avgHours = totalCount > 0 ? totalHours / totalCount : 0;

            TxtFleetCount.Text = totalCount.ToString();
            TxtFleetValue.Text = $"€{totalValue:N0}";
            TxtFleetHours.Text = $"{totalHours:N1}h";
            TxtFleetMaintenance.Text = maintenanceCount.ToString();
            TxtFleetAvgHours.Text = $"{avgHours:N1}h";

            FleetGrid.ItemsSource = aircraft.OrderBy(a => a.Registration).ToList();

            LoadFleetSizePieChart(aircraft);
            LoadFleetStatusPieChart(aircraft);
        }

        private void LoadFleetSizePieChart(List<Models.Aircraft> aircraft)
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White
            };

            if (aircraft.Count == 0)
            {
                FleetSizePieChart.Model = plotModel;
                return;
            }

            var categoryGroups = aircraft
                .GroupBy(a => a.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderBy(g => g.Category)
                .ToList();

            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}",
                OutsideLabelFormat = "{0}",
                TextColor = OxyColors.White,
                FontSize = 10
            };

            var categoryColors = new Dictionary<AircraftCategory, OxyColor>
            {
                { AircraftCategory.SingleEnginePiston, OxyColor.FromRgb(76, 175, 80) },
                { AircraftCategory.MultiEnginePiston, OxyColor.FromRgb(139, 195, 74) },
                { AircraftCategory.Turboprop, OxyColor.FromRgb(33, 150, 243) },
                { AircraftCategory.BusinessJet, OxyColor.FromRgb(156, 39, 176) },
                { AircraftCategory.RegionalJet, OxyColor.FromRgb(255, 152, 0) },
                { AircraftCategory.NarrowBody, OxyColor.FromRgb(244, 67, 54) },
                { AircraftCategory.WideBody, OxyColor.FromRgb(233, 30, 99) }
            };

            foreach (var group in categoryGroups)
            {
                var color = categoryColors.GetValueOrDefault(group.Category, OxyColors.Gray);
                var label = GetCategoryShortName(group.Category);
                pieSeries.Slices.Add(new PieSlice(label, group.Count) { Fill = color });
            }

            plotModel.Series.Add(pieSeries);
            FleetSizePieChart.Model = plotModel;
        }

        private static string GetCategoryShortName(AircraftCategory category)
        {
            return category switch
            {
                AircraftCategory.SingleEnginePiston => "SEP",
                AircraftCategory.MultiEnginePiston => "MEP",
                AircraftCategory.Turboprop => "Turbo",
                AircraftCategory.BusinessJet => "BizJet",
                AircraftCategory.RegionalJet => "Regional",
                AircraftCategory.NarrowBody => "Narrow",
                AircraftCategory.WideBody => "Wide",
                _ => category.ToString()
            };
        }

        private void LoadFleetStatusPieChart(List<Models.Aircraft> aircraft)
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White
            };

            if (aircraft.Count == 0)
            {
                FleetStatusPieChart.Model = plotModel;
                return;
            }

            var statusGroups = aircraft
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .OrderBy(g => g.Status)
                .ToList();

            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}",
                OutsideLabelFormat = "{0}",
                TextColor = OxyColors.White,
                FontSize = 11
            };

            var statusColors = new Dictionary<AircraftStatus, OxyColor>
            {
                { AircraftStatus.Available, OxyColor.FromRgb(76, 175, 80) },
                { AircraftStatus.InFlight, OxyColor.FromRgb(33, 150, 243) },
                { AircraftStatus.Maintenance, OxyColor.FromRgb(255, 152, 0) },
                { AircraftStatus.Stationed, OxyColor.FromRgb(156, 39, 176) },
                { AircraftStatus.Grounded, OxyColor.FromRgb(244, 67, 54) }
            };

            foreach (var group in statusGroups)
            {
                var color = statusColors.GetValueOrDefault(group.Status, OxyColors.Gray);
                pieSeries.Slices.Add(new PieSlice(group.Status.ToString(), group.Count) { Fill = color });
            }

            plotModel.Series.Add(pieSeries);
            FleetStatusPieChart.Model = plotModel;
        }

        private void LoadFinanceStats()
        {
            var transactions = _transactionRepository.GetAllTransactions();

            var totalRevenue = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount));
            var netProfit = totalRevenue - totalExpenses;
            var currentBalance = _financeService.Balance;

            var monthlyData = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .Select(g => new
                {
                    Revenue = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = g.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount))
                })
                .ToList();

            var avgMonthlyProfit = monthlyData.Count > 0
                ? monthlyData.Average(m => m.Revenue - m.Expenses)
                : 0;

            TxtTotalRevenue.Text = $"€{totalRevenue:N0}";
            TxtTotalExpenses.Text = $"€{totalExpenses:N0}";
            TxtNetProfit.Text = $"€{netProfit:N0}";
            TxtCurrentBalance.Text = $"€{currentBalance:N0}";
            TxtAvgMonthlyProfit.Text = $"€{avgMonthlyProfit:N0}";

            var recentTransactions = transactions
                .OrderByDescending(t => t.Date)
                .Take(50)
                .ToList();

            TransactionsGrid.ItemsSource = recentTransactions;

            LoadRevenueExpenseChart(transactions);
            LoadMonthlyProfitChart(transactions);
        }

        private void LoadMonthlyProfitChart(List<Transaction> transactions)
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.Gray
            };

            if (transactions.Count == 0)
            {
                MonthlyProfitChart.Model = plotModel;
                return;
            }

            var monthlyData = transactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Revenue = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = g.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount)),
                    Profit = g.Where(t => t.Type == "Income").Sum(t => t.Amount) -
                             g.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount))
                })
                .TakeLast(12)
                .ToList();

            var barSeries = new BarSeries
            {
                StrokeThickness = 0,
                TrackerFormatString = "{Category}: €{Value:N0}"
            };

            foreach (var month in monthlyData)
            {
                var color = month.Profit >= 0
                    ? OxyColor.FromRgb(76, 175, 80)
                    : OxyColor.FromRgb(244, 67, 54);
                barSeries.Items.Add(new BarItem { Value = (double)month.Profit, Color = color });
            }

            plotModel.Series.Add(barSeries);

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                GapWidth = 0.2
            };

            foreach (var month in monthlyData)
            {
                categoryAxis.Labels.Add(month.Date.ToString("MMM yy"));
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(50, 50, 50),
                LabelFormatter = value => FormatCurrency(value)
            };

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            MonthlyProfitChart.Model = plotModel;
        }

        private void LoadRevenueExpenseChart(List<Transaction> transactions)
        {
            var plotModel = new PlotModel
            {
                Background = OxyColors.Transparent,
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.Gray
            };

            var validTransactions = transactions
                .Where(t => t.Date.Year >= 2000 && t.Date <= DateTime.Now.AddMonths(1))
                .ToList();

            if (validTransactions.Count == 0)
            {
                RevenueExpenseChartModel = plotModel;
                return;
            }

            var groupedByMonth = validTransactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Revenue = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expenses = g.Where(t => t.Type == "Expense").Sum(t => Math.Abs(t.Amount))
                })
                .ToList();

            if (groupedByMonth.Count == 0)
            {
                RevenueExpenseChartModel = plotModel;
                return;
            }

            var revenueSeries = new AreaSeries
            {
                Title = "Revenue",
                Color = OxyColor.FromRgb(76, 175, 80),
                Fill = OxyColor.FromArgb(80, 76, 175, 80),
                StrokeThickness = 2
            };

            var expensesSeries = new AreaSeries
            {
                Title = "Expenses",
                Color = OxyColor.FromRgb(244, 67, 54),
                Fill = OxyColor.FromArgb(80, 244, 67, 54),
                StrokeThickness = 2
            };

            double totalRev = 0;
            double totalExp = 0;

            var firstDate = groupedByMonth.First().Date;
            var startDate = firstDate.AddDays(-1);
            revenueSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(startDate), 0));
            expensesSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(startDate), 0));

            foreach (var item in groupedByMonth)
            {
                totalRev += (double)item.Revenue;
                totalExp += (double)item.Expenses;
                revenueSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), totalRev));
                expensesSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), totalExp));
            }

            plotModel.Series.Add(expensesSeries);
            plotModel.Series.Add(revenueSeries);

            var minDate = startDate.AddDays(-5);
            var maxDate = groupedByMonth.Last().Date.AddMonths(1);

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                StringFormat = "MMM yy",
                Angle = 45,
                IntervalType = DateTimeIntervalType.Months,
                MinorIntervalType = DateTimeIntervalType.Months,
                Minimum = DateTimeAxis.ToDouble(minDate),
                Maximum = DateTimeAxis.ToDouble(maxDate)
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.Gray,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(50, 50, 50),
                Minimum = 0,
                LabelFormatter = value => FormatCurrency(value)
            };

            plotModel.Axes.Add(dateAxis);
            plotModel.Axes.Add(valueAxis);

            plotModel.IsLegendVisible = true;

            RevenueExpenseChartModel = plotModel;
        }

        private static string FormatCurrency(double value)
        {
            if (value >= 1_000_000)
                return $"€{value / 1_000_000:F1}M";
            else if (value >= 1_000)
                return $"€{value / 1_000:F0}K";
            else
                return $"€{value:F0}";
        }

        private void LoadPilotStats()
        {
            var pilots = _pilotRepository.GetEmployedPilots();
            var aircraft = _aircraftRepository.GetAllAircraft();
            var settings = _settingsService.CurrentSettings;

            var pilotCount = pilots.Count;
            // Calculate salaries dynamically based on rank and settings (player pilot excluded)
            var totalSalaries = pilots.Where(p => !p.IsPlayer).Sum(p =>
            {
                var rank = PilotRank.GetRank(p.TotalFlightHours, settings);
                return PilotRank.CalculateAdjustedSalary(rank, settings);
            });
            var totalHours = pilots.Sum(p => p.TotalFlightHours);
            var assignedCount = aircraft.Count(a => a.AssignedPilotId != null);

            TxtPilotCount.Text = pilotCount.ToString();
            TxtPilotSalaries.Text = $"€{totalSalaries:N0}";
            TxtPilotHours.Text = $"{totalHours:N1}h";
            TxtPilotsAssigned.Text = assignedCount.ToString();

            var pilotStats = pilots.Select(p =>
            {
                var rank = PilotRank.GetRank(p.TotalFlightHours, settings);
                // Player pilot doesn't get salary
                var adjustedSalary = p.IsPlayer ? 0m : PilotRank.CalculateAdjustedSalary(rank, settings);
                return new PilotStatViewModel
                {
                    Name = p.Name,
                    TotalFlightHours = p.TotalFlightHours,
                    SalaryPerMonth = adjustedSalary,
                    Rank = PilotRank.GetRankName(rank),
                    AssignedAircraftReg = aircraft.FirstOrDefault(a => a.AssignedPilotId == p.Id)?.Registration ?? "-",
                    StatusText = p.IsPlayer ? "Player" : (aircraft.Any(a => a.AssignedPilotId == p.Id) ? "Assigned" : "Available")
                };
            }).OrderBy(p => p.Name).ToList();

            PilotsGrid.ItemsSource = pilotStats;
        }

        private void StreetLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Street);
        }

        private void TerrainLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Terrain);
        }

        private void SatelliteLayerButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchMapLayer(MapLayerType.Satellite);
        }

        private void SwitchMapLayer(MapLayerType layerType)
        {
            _flightMapViewModel.RefreshMapLayer(layerType);

            _settingsService.CurrentSettings.MapLayer = layerType.ToString();
            _settingsService.Save();

            UpdateMapLayerButtons(layerType);
            UpdateLegendVisibility(layerType);
        }

        private void UpdateMapLayerButtons(MapLayerType layerType = MapLayerType.Street)
        {
            StreetLayerButton.Opacity = layerType == MapLayerType.Street ? 1.0 : 0.6;
            TerrainLayerButton.Opacity = layerType == MapLayerType.Terrain ? 1.0 : 0.6;
            SatelliteLayerButton.Opacity = layerType == MapLayerType.Satellite ? 1.0 : 0.6;
        }

        private void UpdateLegendVisibility(MapLayerType layerType)
        {
            LegendPanel.ShowTerrainLegend(layerType == MapLayerType.Terrain);
        }
    }

    public class PilotStatViewModel
    {
        public string Name { get; set; } = string.Empty;
        public double TotalFlightHours { get; set; }
        public decimal SalaryPerMonth { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string AssignedAircraftReg { get; set; } = "-";
        public string StatusText { get; set; } = string.Empty;
    }

    public class AircraftPerformanceViewModel
    {
        public string Registration { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int DaysActive { get; set; }
        public double TotalHours { get; set; }
        public double TotalDistance { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal TotalProfit { get; set; }
        public bool IsProfitNegative => TotalProfit < 0;
        public double ProfitMargin => TotalRevenue > 0 ? (double)(TotalProfit / TotalRevenue) * 100 : 0;
    }
}
