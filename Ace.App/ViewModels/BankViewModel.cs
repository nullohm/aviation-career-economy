using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.ViewModels
{
    public class BankViewModel : ViewModelBase
    {
        private readonly IFinanceService _financeService;
        private readonly ILoanService _loanService;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IFBORepository _fboRepository;
        private readonly IPilotRepository _pilotRepository;

        private decimal _loanAmount;
        private string _outstandingLoansText = "€ 0.00";
        private string _monthlyFBOCostsText = "€ 0.00";
        private string _monthlyPilotCostsText = "€ 0.00";
        private decimal _totalAircraftValue;
        private ObservableCollection<LoanViewModel> _activeLoans = new();
        private ObservableCollection<FBOCostViewModel> _rentedFBOs = new();
        private ObservableCollection<PilotCostViewModel> _employedPilots = new();

        public decimal LoanAmount
        {
            get => _loanAmount;
            set => SetProperty(ref _loanAmount, value);
        }

        public string OutstandingLoansText
        {
            get => _outstandingLoansText;
            set => SetProperty(ref _outstandingLoansText, value);
        }

        public ObservableCollection<LoanViewModel> ActiveLoans
        {
            get => _activeLoans;
            set => SetProperty(ref _activeLoans, value);
        }

        public string MonthlyFBOCostsText
        {
            get => _monthlyFBOCostsText;
            set => SetProperty(ref _monthlyFBOCostsText, value);
        }

        public ObservableCollection<FBOCostViewModel> RentedFBOs
        {
            get => _rentedFBOs;
            set => SetProperty(ref _rentedFBOs, value);
        }

        public string MonthlyPilotCostsText
        {
            get => _monthlyPilotCostsText;
            set => SetProperty(ref _monthlyPilotCostsText, value);
        }

        public ObservableCollection<PilotCostViewModel> EmployedPilots
        {
            get => _employedPilots;
            set => SetProperty(ref _employedPilots, value);
        }

        public string BalanceFormatted => _financeService.BalanceFormatted;

        public decimal Balance => _financeService.Balance;

        public decimal TotalAircraftValue
        {
            get => _totalAircraftValue;
            private set
            {
                if (SetProperty(ref _totalAircraftValue, value))
                {
                    OnPropertiesChanged(
                        nameof(TotalAircraftValueFormatted),
                        nameof(TotalAssets),
                        nameof(TotalAssetsFormatted));
                }
            }
        }

        public string TotalAircraftValueFormatted => $"€ {_totalAircraftValue:N0}";

        public decimal TotalAssets => _financeService.Balance + _totalAircraftValue;

        public string TotalAssetsFormatted => $"€ {TotalAssets:N0}";

        public System.Collections.Generic.List<Transaction> RecentTransactions => _financeService.RecentTransactions;

        public BankViewModel(
            IFinanceService financeService,
            ILoanService loanService,
            ILoggingService logger,
            ISettingsService settingsService,
            IAircraftRepository aircraftRepository,
            ILoanRepository loanRepository,
            IFBORepository fboRepository,
            IPilotRepository pilotRepository)
        {
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _loanService = loanService ?? throw new ArgumentNullException(nameof(loanService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _loanRepository = loanRepository ?? throw new ArgumentNullException(nameof(loanRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));

            _loanService.LoansChanged += OnLoansChanged;
            _financeService.PropertyChanged += OnFinanceChanged;
            LoadLoans();
            LoadFBOs();
            LoadPilots();
            LoadAircraftValue();
        }

        private void OnFinanceChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IFinanceService.Balance))
            {
                OnPropertiesChanged(
                    nameof(BalanceFormatted),
                    nameof(Balance),
                    nameof(TotalAssets),
                    nameof(TotalAssetsFormatted));
            }
        }

        private void OnLoansChanged()
        {
            LoadLoans();
        }

        public void LoadAircraftValue()
        {
            var totalValue = _aircraftRepository.GetTotalFleetValue();
            TotalAircraftValue = totalValue;
            _logger.Debug($"BankViewModel: Total aircraft value: €{totalValue:N0}");
        }

        public void LoadLoans()
        {
            var loans = _loanRepository.GetActiveLoans();

            ActiveLoans.Clear();
            foreach (var loan in loans)
            {
                ActiveLoans.Add(new LoanViewModel(loan));
            }

            var outstanding = _loanService.GetTotalOutstandingLoans();
            OutstandingLoansText = $"€ {outstanding:N2}".Replace(",", ".");

            _logger.Debug($"BankViewModel: Loaded {ActiveLoans.Count} active loans, total outstanding: {outstanding:C}");
        }

        public void LoadFBOs()
        {
            var fbos = _fboRepository.GetAllFBOs().OrderBy(f => f.ICAO).ToList();

            RentedFBOs.Clear();
            decimal totalMonthlyCost = 0;

            foreach (var fbo in fbos)
            {
                var vm = new FBOCostViewModel(fbo);
                RentedFBOs.Add(vm);
                totalMonthlyCost += vm.TotalMonthlyCost;
            }

            MonthlyFBOCostsText = $"€ {totalMonthlyCost:N2}";

            _logger.Debug($"BankViewModel: Loaded {RentedFBOs.Count} FBOs, total monthly cost: €{totalMonthlyCost:N2}");
        }

        public void LoadPilots()
        {
            var pilots = _pilotRepository.GetEmployedNonPlayerPilots();
            var settings = _settingsService.CurrentSettings;

            EmployedPilots.Clear();
            decimal totalMonthlyCost = 0;

            foreach (var pilot in pilots)
            {
                var vm = new PilotCostViewModel(pilot, settings);
                EmployedPilots.Add(vm);
                totalMonthlyCost += vm.SalaryPerMonth;
            }

            MonthlyPilotCostsText = $"€ {totalMonthlyCost:N2}";

            _logger.Debug($"BankViewModel: Loaded {EmployedPilots.Count} employed pilots, total monthly cost: €{totalMonthlyCost:N2}");
        }
    }

    public class LoanViewModel
    {
        private readonly Loan _loan;

        public LoanViewModel(Loan loan)
        {
            _loan = loan;
        }

        public int Id => _loan.Id;
        public string AmountText => $"€ {_loan.Amount:N0}";
        public string RepaymentText => $"€ {_loan.TotalRepayment:N0}";
        public string DateText => _loan.TakenDate.ToString("dd.MM.yyyy HH:mm");
        public string InterestText => $"{(_loan.InterestRate * 100):F0}%";
    }

    public class FBOCostViewModel
    {
        private readonly FBO _fbo;

        public FBOCostViewModel(FBO fbo)
        {
            _fbo = fbo;
        }

        public int Id => _fbo.Id;
        public string ICAO => _fbo.ICAO;
        public string AirportName => _fbo.AirportName;
        public string TypeText => _fbo.Type.ToString();
        public string RentedSinceText => _fbo.RentedSince.ToString("dd.MM.yyyy");
        public decimal MonthlyRent => _fbo.MonthlyRent;
        public decimal TerminalCost => _fbo.TerminalMonthlyCost;
        public decimal TotalMonthlyCost => _fbo.MonthlyRent + _fbo.TerminalMonthlyCost;
        public string MonthlyRentText => $"€ {MonthlyRent:N0}";
        public string TerminalCostText => TerminalCost > 0 ? $"€ {TerminalCost:N0}" : "-";
        public string TotalCostText => $"€ {TotalMonthlyCost:N0}";
        public string DisplayText => $"{ICAO} - {AirportName}";
        public string TerminalSizeText => _fbo.TerminalSize == TerminalSize.None ? "-" : _fbo.TerminalSize.ToString();
    }

    public class PilotCostViewModel
    {
        private readonly Pilot _pilot;
        private readonly PilotRankType _rank;
        private readonly decimal _calculatedSalary;

        public PilotCostViewModel(Pilot pilot, AppSettings? settings)
        {
            _pilot = pilot;
            _rank = settings != null
                ? PilotRank.GetRank(pilot.TotalFlightHours, settings)
                : PilotRankType.Junior;
            // Player pilot doesn't get salary
            _calculatedSalary = pilot.IsPlayer ? 0m : (settings != null
                ? PilotRank.CalculateAdjustedSalary(_rank, settings)
                : 0m);
        }

        public int Id => _pilot.Id;
        public string Name => _pilot.Name;
        public decimal SalaryPerMonth => _calculatedSalary;
        public string SalaryText => $"€ {SalaryPerMonth:N0}";
        public string FlightHoursText => $"{_pilot.TotalFlightHours:N0} hrs";
        public string RankText => PilotRank.GetRankName(_rank);
    }
}
