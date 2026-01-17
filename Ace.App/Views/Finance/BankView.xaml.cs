using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Finance
{
    public partial class BankView : UserControl
    {
        private readonly BankViewModel _viewModel;
        private readonly ILoanService _loanService;
        private readonly ILoggingService _logger;
        private readonly IPricingService _pricingService;
        private readonly IFlightRepository _flightRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDailyEarningsRepository _dailyEarningsRepository;
        private readonly IMonthlyBillingRepository _monthlyBillingRepository;
        private bool _costsExpanded = true;

        public BankView(
            BankViewModel viewModel,
            ILoanService loanService,
            ILoggingService logger,
            IPricingService pricingService,
            IFlightRepository flightRepository,
            IAircraftRepository aircraftRepository,
            ITransactionRepository transactionRepository,
            IDailyEarningsRepository dailyEarningsRepository,
            IMonthlyBillingRepository monthlyBillingRepository)
        {
            InitializeComponent();
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _loanService = loanService ?? throw new ArgumentNullException(nameof(loanService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _dailyEarningsRepository = dailyEarningsRepository ?? throw new ArgumentNullException(nameof(dailyEarningsRepository));
            _monthlyBillingRepository = monthlyBillingRepository ?? throw new ArgumentNullException(nameof(monthlyBillingRepository));
            DataContext = _viewModel;

            _logger.Debug("BankView: Constructor");

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _logger.Debug("BankView: Loaded event");
            _viewModel.LoadPilots();
            _viewModel.LoadFBOs();
            _viewModel.LoadLoans();
            _viewModel.LoadAircraftValue();
            UpdateStats();
            UpdateLoanHint();
        }

        private void UpdateStats()
        {
            TxtTotalAssets.Text = $"Total: {_viewModel.TotalAssetsFormatted}";
            TxtBalance.Text = $"Cash: {_viewModel.BalanceFormatted}";
            TxtAircraftValue.Text = $"Fleet: {_viewModel.TotalAircraftValueFormatted}";
            TxtLoans.Text = $"Loans: {_viewModel.OutstandingLoansText}";
            TxtCostsHeader.Text = $"Monthly Costs ({_viewModel.RentedFBOs.Count + _viewModel.EmployedPilots.Count})";
            TxtLoansHeader.Text = $"Active Loans ({_viewModel.ActiveLoans.Count})";
            TxtHintFBO.Text = $"FBO {_viewModel.MonthlyFBOCostsText}";
            TxtHintPilots.Text = $"Pilots {_viewModel.MonthlyPilotCostsText}";
        }

        private void UpdateLoanHint()
        {
            TxtLoanHint.Visibility = string.IsNullOrEmpty(TxtLoanAmount.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void TxtLoanAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLoanHint();
        }

        private void CostsHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _costsExpanded = !_costsExpanded;
            CostsScrollViewer.Visibility = _costsExpanded ? Visibility.Visible : Visibility.Collapsed;
            TxtCostsExpandIcon.Text = _costsExpanded ? "▼" : "▶";
        }

        private void TakeLoan_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.LoanAmount <= 0)
            {
                _logger.Warn($"BankView: Invalid loan amount: {_viewModel.LoanAmount}");
                var invalidDialog = new MessageDialog("Invalid Amount", "Please enter a valid loan amount.") { Owner = Window.GetWindow(this) };
                invalidDialog.ShowDialog();
                return;
            }

            var confirmDialog = new ConfirmDialog(
                "Take Loan",
                $"Take loan of € {_viewModel.LoanAmount:N0}?\nRepayment amount: € {(_viewModel.LoanAmount * 1.10m):N0} (10% interest)")
            { Owner = Window.GetWindow(this) };
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
            {
                _logger.Info($"BankView: Loan cancelled by user");
                return;
            }

            if (_loanService.TakeLoan(_viewModel.LoanAmount))
            {
                _logger.Info($"BankView: Loan of {_viewModel.LoanAmount:C} taken successfully");
                _viewModel.LoanAmount = 0;
                UpdateStats();
                UpdateLoanHint();
            }
            else
            {
                _logger.Error($"BankView: Failed to take loan of {_viewModel.LoanAmount:C}");
                var errorDialog = new MessageDialog("Error", "Failed to process loan.") { Owner = Window.GetWindow(this) };
                errorDialog.ShowDialog();
            }
        }

        private void RepayLoan_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var loanViewModel = button?.Tag as LoanViewModel;

            if (loanViewModel == null)
            {
                _logger.Warn("BankView: Repay button clicked but no loanViewModel found");
                return;
            }

            _logger.Info($"BankView: Repay button clicked for loan {loanViewModel.Id}");

            var confirmDialog = new ConfirmDialog(
                "Repay Loan",
                $"Repay loan of {loanViewModel.RepaymentText}?")
            { Owner = Window.GetWindow(this) };
            confirmDialog.ShowDialog();

            if (!confirmDialog.Result)
            {
                _logger.Info($"BankView: Loan repayment cancelled by user for loan {loanViewModel.Id}");
                return;
            }

            if (_loanService.RepayLoan(loanViewModel.Id))
            {
                _logger.Info($"BankView: Loan {loanViewModel.Id} repaid successfully");
                UpdateStats();
            }
            else
            {
                _logger.Warn($"BankView: Failed to repay loan {loanViewModel.Id}");
                var errorDialog = new MessageDialog("Repayment Failed", "Failed to repay loan. Check if you have sufficient balance.") { Owner = Window.GetWindow(this) };
                errorDialog.ShowDialog();
            }
        }

        private void Transaction_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var transaction = border?.Tag as Transaction;

            if (transaction == null)
                return;

            if (transaction.Description.StartsWith("Daily earnings:"))
            {
                var datePart = transaction.Description.Split(':').LastOrDefault()?.Trim();
                if (DateTime.TryParseExact(datePart, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    _logger.Info($"BankView: Opening daily earnings details for {date:dd.MM.yyyy}");
                    var detailWindow = new DailyEarningsDetailWindow(date, _dailyEarningsRepository)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    detailWindow.ShowDialog();
                }
            }
            else if (transaction.Description.StartsWith("Monthly costs:"))
            {
                var datePart = transaction.Description.Split(':').LastOrDefault()?.Trim();
                if (DateTime.TryParseExact(datePart, "MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    _logger.Info($"BankView: Opening monthly billing details for {date:MM.yyyy}");
                    var detailWindow = new MonthlyBillingDetailWindow(date, _monthlyBillingRepository)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    detailWindow.ShowDialog();
                }
            }
            else if (transaction.Description.StartsWith("Monthly summary:"))
            {
                var datePart = transaction.Description.Split(':').LastOrDefault()?.Trim();
                if (DateTime.TryParseExact(datePart, "MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    _logger.Info($"BankView: Opening monthly earnings report for {date:MM.yyyy}");
                    var reportWindow = new MonthlyEarningsReportWindow(date.Year, date.Month, _dailyEarningsRepository)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    reportWindow.ShowDialog();
                }
            }
            else if (transaction.Description.StartsWith("Yearly summary:"))
            {
                var datePart = transaction.Description.Split(':').LastOrDefault()?.Trim();
                if (int.TryParse(datePart, out int year))
                {
                    _logger.Info($"BankView: Opening yearly earnings report for {year}");
                    var reportWindow = new YearlyEarningsReportWindow(year, _dailyEarningsRepository)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    reportWindow.ShowDialog();
                }
            }
            else if (transaction.FlightId.HasValue)
            {
                _logger.Info($"BankView: Opening flight details for flight ID {transaction.FlightId}");
                var detailWindow = new FlightDetailDialog(
                    transaction.FlightId.Value,
                    _logger,
                    _pricingService,
                    _flightRepository,
                    _aircraftRepository,
                    _transactionRepository)
                {
                    Owner = Window.GetWindow(this)
                };
                detailWindow.ShowDialog();
            }
        }
    }
}
