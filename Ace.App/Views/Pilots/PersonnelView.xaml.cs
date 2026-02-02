using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;

namespace Ace.App.Views.Pilots
{
    public partial class PersonnelView : UserControl
    {
        private readonly PersonnelViewModel _viewModel;
        private readonly ILoggingService _logger;
        private readonly ISettingsService _settingsService;
        private readonly IFinanceService _financeService;
        private readonly IPilotRepository _pilotRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ITypeRatingRepository _typeRatingRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private PilotViewModel? _selectedPilot;
        private bool _listExpanded = true;
        private bool _showingEmployed = true;
        private bool _isLoaded = false;

        public PersonnelView(
            PersonnelViewModel viewModel,
            ILoggingService logger,
            ISettingsService settingsService,
            IFinanceService financeService,
            IPilotRepository pilotRepository,
            ILicenseRepository licenseRepository,
            ITypeRatingRepository typeRatingRepository,
            IAircraftRepository aircraftRepository,
            IAircraftPilotAssignmentRepository assignmentRepository,
            ITransactionRepository transactionRepository)
        {
            InitializeComponent();

            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _licenseRepository = licenseRepository ?? throw new ArgumentNullException(nameof(licenseRepository));
            _typeRatingRepository = typeRatingRepository ?? throw new ArgumentNullException(nameof(typeRatingRepository));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            DataContext = _viewModel;

            _logger.Debug($"PersonnelView: Constructor - EmployedPilots={_viewModel.EmployedPilots.Count}, AvailablePilots={_viewModel.AvailablePilots.Count}");

            _viewModel.HireRequested += OnHireRequested;
            _viewModel.HireCompleted += OnHireCompleted;
            _viewModel.FireRequested += OnFireRequested;
            _viewModel.FireCompleted += OnFireCompleted;
            _viewModel.EditRequested += OnEditRequested;
            _viewModel.EditCompleted += OnEditCompleted;
            _viewModel.DeleteRequested += OnDeleteRequested;
            _viewModel.DeleteCompleted += OnDeleteCompleted;
            _viewModel.CreateRequested += OnCreateRequested;
            _viewModel.CreateCompleted += OnCreateCompleted;
            _viewModel.ErrorOccurred += OnErrorOccurred;

            Loaded += PersonnelView_Loaded;
        }

        private void PersonnelView_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Debug($"PersonnelView: Loaded event - EmployedPilots={_viewModel.EmployedPilots.Count}, AvailablePilots={_viewModel.AvailablePilots.Count}");
            UpdatePilotList();
            UpdateStats();
            _isLoaded = true;
            _logger.Debug($"PersonnelView: Loaded complete - ItemsSource set, _isLoaded={_isLoaded}");
        }

        private void CmbViewToggle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null || !_isLoaded) return;

            _showingEmployed = CmbViewToggle.SelectedIndex == 0;
            UpdatePilotList();
            UpdateStats();

            BtnCreatePilot.Visibility = _showingEmployed ? Visibility.Collapsed : Visibility.Visible;

            HideDetails();
            _selectedPilot = null;
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel == null || !_isLoaded || CmbSort.SelectedIndex < 0) return;

            if (_showingEmployed)
            {
                _viewModel.SortEmployedPilots(CmbSort.SelectedIndex);
            }
            else
            {
                _viewModel.SortAvailablePilots(CmbSort.SelectedIndex);
            }
        }

        private void PilotListHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _listExpanded = !_listExpanded;
            PilotListScrollViewer.Visibility = _listExpanded ? Visibility.Visible : Visibility.Collapsed;
            TxtExpandIcon.Text = _listExpanded ? "▼" : "▶";
        }

        private void OnPilotListItemClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is PilotViewModel pilot)
            {
                SelectPilot(pilot);
            }
        }

        private void SelectPilot(PilotViewModel pilot)
        {
            _selectedPilot = pilot;
            UpdateDetailsPanel();
            ShowDetails();
        }

        private void ShowDetails()
        {
            TxtSelectHint.Visibility = Visibility.Collapsed;
            HeaderContent.Visibility = Visibility.Visible;
            DetailsContent.Visibility = Visibility.Visible;
        }

        private void HideDetails()
        {
            TxtSelectHint.Visibility = Visibility.Visible;
            HeaderContent.Visibility = Visibility.Collapsed;
            DetailsContent.Visibility = Visibility.Collapsed;
        }

        private void UpdateDetailsPanel()
        {
            if (_selectedPilot == null) return;

            TxtDetailName.Text = _selectedPilot.Name;
            DetailRankBadge.Rank = _selectedPilot.Rank;
            DetailRankBadge.ToolTip = _selectedPilot.RankName;
            DetailPlayerBadge.Visibility = _selectedPilot.IsPlayer
                ? Visibility.Visible : Visibility.Collapsed;
            TxtDetailStatus.Text = _selectedPilot.HasStatus
                ? _selectedPilot.StatusDetail : "";

            TxtDetailAge.Text = $"{_selectedPilot.Age} years";
            TxtDetailExperience.Text = $"{_selectedPilot.TotalFlightHours:N0} hours";
            TxtDetailRankFull.Text = _selectedPilot.RankName;

            SalarySection.Visibility = _selectedPilot.IsPlayer
                ? Visibility.Collapsed : Visibility.Visible;
            TxtDetailSalary.Text = $"{_selectedPilot.AdjustedSalary:N0} €/mo";

            TxtDetailBirthday.Text = _selectedPilot.Birthday.ToString("dd.MM.yyyy");
            TxtDetailEmployed.Text = _selectedPilot.IsEmployed ? "Yes" : "No";

            UpdatePilotImage();

            LoadLicenses();
            LoadTypeRatings();
            LoadAircraftAssignment();

            ConfigureHireFireButton();

            BtnDelete.Visibility = _selectedPilot.IsPlayer
                ? Visibility.Collapsed : Visibility.Visible;
        }

        private void LoadLicenses()
        {
            if (_selectedPilot == null) return;

            var licenses = _licenseRepository.GetLicensesByPilotId(_selectedPilot.Id);

            LicensesList.ItemsSource = licenses;
            TxtNoLicenses.Visibility = licenses.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void LoadTypeRatings()
        {
            if (_selectedPilot == null) return;

            var typeRatings = _typeRatingRepository.GetTypeRatingsByPilotId(_selectedPilot.Id);

            TypeRatingsList.ItemsSource = typeRatings;
            TxtNoTypeRatings.Visibility = typeRatings.Any() ? Visibility.Collapsed : Visibility.Visible;

            var isEmployed = _selectedPilot.IsEmployed;
            PurchaseTypeRatingSection.Visibility = isEmployed ? Visibility.Visible : Visibility.Collapsed;

            if (isEmployed)
            {
                LoadAvailableTypeRatings(typeRatings);
            }
        }

        private void LoadAvailableTypeRatings(List<TypeRating> existingRatings)
        {
            if (_selectedPilot == null) return;

            var ownedAircraftTypes = _aircraftRepository.GetDistinctAircraftTypes();

            var existingTypes = existingRatings.Select(r => r.AircraftType).ToHashSet();

            var availableRatings = ownedAircraftTypes
                .Where(t => !existingTypes.Contains(t))
                .Select(t => new TypeRatingOption { AircraftType = t, Price = CalculateTypeRatingPrice(t) })
                .OrderBy(t => t.AircraftType)
                .ToList();

            CmbTypeRatings.ItemsSource = availableRatings;
            CmbTypeRatings.DisplayMemberPath = "DisplayText";

            if (availableRatings.Any())
            {
                CmbTypeRatings.SelectedIndex = 0;
            }
        }

        private decimal CalculateTypeRatingPrice(string aircraftType)
        {
            var settings = _settingsService.CurrentSettings;

            var ownedAircraft = _aircraftRepository.GetAircraftByType(aircraftType);
            if (ownedAircraft != null)
            {
                var aircraftSize = AircraftSizeExtensions.GetAircraftSize(ownedAircraft.MaxPassengers);
                return aircraftSize switch
                {
                    AircraftSize.Small => settings.TypeRatingCostSmall,
                    AircraftSize.Medium => settings.TypeRatingCostMedium,
                    AircraftSize.MediumLarge => settings.TypeRatingCostMediumLarge,
                    AircraftSize.Large => settings.TypeRatingCostLarge,
                    AircraftSize.VeryLarge => settings.TypeRatingCostVeryLarge,
                    _ => settings.TypeRatingCostMedium
                };
            }

            return settings.TypeRatingCostMedium;
        }

        private void LoadAircraftAssignment()
        {
            if (_selectedPilot == null) return;

            var isEmployed = _selectedPilot.IsEmployed;
            AssignmentSection.Visibility = isEmployed ? Visibility.Visible : Visibility.Collapsed;

            if (!isEmployed) return;

            var assignedAircraft = _aircraftRepository.GetAircraftByPilotId(_selectedPilot.Id);

            if (assignedAircraft != null)
            {
                CurrentAssignmentPanel.Visibility = Visibility.Visible;
                AssignAircraftPanel.Visibility = Visibility.Collapsed;
                TxtNoAircraftAvailable.Visibility = Visibility.Collapsed;

                TxtAssignedAircraft.Text = assignedAircraft.Registration;
                TxtAssignedAircraftType.Text = $"{assignedAircraft.Type} - {assignedAircraft.Variant}";
            }
            else
            {
                CurrentAssignmentPanel.Visibility = Visibility.Collapsed;

                var pilotTypeRatings = _typeRatingRepository.GetTypeRatingsByPilotId(_selectedPilot.Id)
                    .Select(t => t.AircraftType)
                    .ToList();

                var stationedAircraft = _aircraftRepository.GetUnassignedStationedAircraft();

                var availableAircraft = stationedAircraft
                    .Where(a => pilotTypeRatings.Any(rating =>
                        a.Type.Contains(rating, StringComparison.OrdinalIgnoreCase) ||
                        rating.Contains(a.Type, StringComparison.OrdinalIgnoreCase) ||
                        a.Variant.Contains(rating, StringComparison.OrdinalIgnoreCase) ||
                        rating.Contains(a.Variant, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                if (availableAircraft.Any())
                {
                    AssignAircraftPanel.Visibility = Visibility.Visible;
                    TxtNoAircraftAvailable.Visibility = Visibility.Collapsed;

                    CmbAvailableAircraft.ItemsSource = availableAircraft
                        .Select(a => new AircraftOption
                        {
                            Id = a.Id,
                            Registration = a.Registration,
                            Type = a.Type,
                            DisplayText = $"{a.Registration} - {a.Type}"
                        })
                        .ToList();
                    CmbAvailableAircraft.DisplayMemberPath = "DisplayText";

                    if (CmbAvailableAircraft.Items.Count > 0)
                    {
                        CmbAvailableAircraft.SelectedIndex = 0;
                    }
                }
                else
                {
                    AssignAircraftPanel.Visibility = Visibility.Collapsed;
                    TxtNoAircraftAvailable.Visibility = Visibility.Visible;
                }
            }
        }

        private void BuyTypeRating_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null || CmbTypeRatings.SelectedItem is not TypeRatingOption selected) return;

            if (_financeService.Balance < selected.Price)
            {
                var errorDialog = new MessageDialog("Insufficient Funds", $"You need {selected.Price:N0} € to purchase this type rating.");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
                return;
            }

            var confirmDialog = new ConfirmDialog(
                $"Purchase {selected.AircraftType} Type Rating?",
                $"Cost: {selected.Price:N0} €"
            );
            confirmDialog.Owner = Window.GetWindow(this);

            if (confirmDialog.ShowDialog() != true) return;

            var typeRating = new TypeRating
            {
                PilotId = _selectedPilot.Id,
                AircraftType = selected.AircraftType,
                EarnedDate = DateTime.Now
            };
            _typeRatingRepository.AddTypeRating(typeRating);

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                Amount = -selected.Price,
                Type = "Training",
                Description = $"Type Rating: {selected.AircraftType} for {_selectedPilot.Name}"
            };
            _transactionRepository.AddTransaction(transaction);

            _financeService.LoadTransactions();

            LoadTypeRatings();
            LoadAircraftAssignment();
        }

        private void AssignAircraft_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null || CmbAvailableAircraft.SelectedItem is not AircraftOption selected) return;

            var aircraft = _aircraftRepository.GetAircraftById(selected.Id);

            if (aircraft == null) return;

            _assignmentRepository.AssignPilot(aircraft.Id, _selectedPilot.Id);

            _logger.Info($"PersonnelView: Assigned {_selectedPilot.Name} to {aircraft.Registration}");

            _viewModel.LoadPilots();
            RefreshSelectedPilot();
        }

        private void UnassignAircraft_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null) return;

            var aircraft = _aircraftRepository.GetAircraftByPilotId(_selectedPilot.Id);

            if (aircraft == null) return;

            _assignmentRepository.UnassignPilot(_selectedPilot.Id);

            _logger.Info($"PersonnelView: Unassigned {_selectedPilot.Name} from {aircraft.Registration}");

            _viewModel.LoadPilots();
            RefreshSelectedPilot();
        }

        private class TypeRatingOption
        {
            public string AircraftType { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string DisplayText => $"{AircraftType} - {Price:N0} €";
        }

        private class AircraftOption
        {
            public int Id { get; set; }
            public string Registration { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string DisplayText { get; set; } = string.Empty;
        }

        private void UpdatePilotImage()
        {
            if (_selectedPilot == null) return;

            if (_selectedPilot.PilotImage != null)
            {
                ImgDetailPilot.Source = _selectedPilot.PilotImage;
                ImgDetailPilot.Visibility = Visibility.Visible;
                DetailPilotIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImgDetailPilot.Visibility = Visibility.Collapsed;
                DetailPilotIcon.Visibility = Visibility.Visible;
            }
        }

        private void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            InfoDialog.Show("Coming Soon", "Pilot image customization will be available in a future update.", Window.GetWindow(this));
        }

        private void ConfigureHireFireButton()
        {
            if (_selectedPilot == null) return;

            if (_showingEmployed)
            {
                if (_selectedPilot.IsPlayer)
                {
                    BtnHireFire.Visibility = Visibility.Collapsed;
                }
                else
                {
                    BtnHireFire.Visibility = Visibility.Visible;
                    BtnHireFire.Content = "Fire";
                    BtnHireFire.Style = (Style)FindResource("DangerButtonStyle");
                }
            }
            else
            {
                BtnHireFire.Visibility = Visibility.Visible;
                BtnHireFire.Content = "Hire";
                BtnHireFire.Style = (Style)FindResource("SuccessButtonStyle");
            }
        }

        private void UpdatePilotList()
        {
            if (_viewModel == null) return;

            var source = _showingEmployed ? _viewModel.EmployedPilots : _viewModel.AvailablePilots;
            _logger.Debug($"PersonnelView: UpdatePilotList - _showingEmployed={_showingEmployed}, source.Count={source.Count}");
            PilotListItems.ItemsSource = source;
        }

        private void UpdateStats()
        {
            if (_viewModel == null) return;

            var employedCount = _viewModel.EmployedPilots.Count;
            var availableCount = _viewModel.AvailablePilots.Count;

            if (_showingEmployed)
            {
                TxtPilotCount.Text = $"{employedCount} Employed";
                TxtPilotListHeader.Text = $"Employed Pilots ({employedCount})";

                var totalSalary = _viewModel.EmployedPilots
                    .Where(p => !p.IsPlayer)
                    .Sum(p => p.AdjustedSalary);
                TxtMonthlyCost.Text = $"€{totalSalary:N0}/mo";
            }
            else
            {
                TxtPilotCount.Text = $"{availableCount} Available";
                TxtPilotListHeader.Text = $"Available Pilots ({availableCount})";
                TxtMonthlyCost.Text = $"Balance: €{_viewModel.Balance:N0}";
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null) return;
            _viewModel.EditPilotCommand.Execute(_selectedPilot);
        }

        private void HireFireButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null) return;

            if (_showingEmployed)
            {
                _viewModel.FirePilotCommand.Execute(_selectedPilot);
            }
            else
            {
                _viewModel.HirePilotCommand.Execute(_selectedPilot);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPilot == null) return;
            _viewModel.DeletePilotCommand.Execute(_selectedPilot);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreatePilotCommand.Execute(null);
        }

        private void RefreshSelectedPilot()
        {
            if (_selectedPilot == null) return;

            var collection = _showingEmployed ? _viewModel.EmployedPilots : _viewModel.AvailablePilots;
            var refreshed = collection.FirstOrDefault(p => p.Id == _selectedPilot.Id);

            if (refreshed != null)
            {
                _selectedPilot = refreshed;
                UpdateDetailsPanel();
            }
            else
            {
                HideDetails();
                _selectedPilot = null;
            }
        }

        private void OnHireRequested(object? sender, PilotActionEventArgs e)
        {
            var dialog = new ConfirmDialog(
                $"Do you want to hire {e.Pilot.Name}?",
                $"Monthly salary: {e.Pilot.SalaryPerMonth:N0} €\nExperience: {e.Pilot.TotalFlightHours:F0} hours"
            );
            dialog.Owner = Window.GetWindow(this);

            e.Confirmed = dialog.ShowDialog() == true;
        }

        private void OnHireCompleted(object? sender, PilotActionEventArgs e)
        {
            if (e.Success)
            {
                UpdateStats();
                UpdatePilotList();
                HideDetails();
                _selectedPilot = null;
            }
        }

        private void OnFireRequested(object? sender, PilotActionEventArgs e)
        {
            var dialog = new ConfirmDialog(
                $"Do you want to fire {e.Pilot.Name}?",
                "This pilot will no longer be part of your team."
            );
            dialog.Owner = Window.GetWindow(this);

            e.Confirmed = dialog.ShowDialog() == true;
        }

        private void OnFireCompleted(object? sender, PilotActionEventArgs e)
        {
            if (e.Success)
            {
                UpdateStats();
                UpdatePilotList();
                HideDetails();
                _selectedPilot = null;
            }
        }

        private void OnErrorOccurred(object? sender, ViewModels.ErrorEventArgs e)
        {
            var errorDialog = new MessageDialog("Error", e.Message);
            errorDialog.Owner = Window.GetWindow(this);
            errorDialog.ShowDialog();
        }

        private void OnEditRequested(object? sender, PilotActionEventArgs e)
        {
            var pilotModel = _pilotRepository.GetPilotById(e.Pilot.Id);

            if (pilotModel == null)
            {
                var errorDialog = new MessageDialog("Error", "Pilot not found.");
                errorDialog.Owner = Window.GetWindow(this);
                errorDialog.ShowDialog();
                e.Confirmed = false;
                return;
            }

            var dialog = new EditPilotDialog(_logger, _settingsService, pilotModel);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.SaveSuccessful && dialog.UpdatedPilot != null)
            {
                _pilotRepository.UpdatePilot(dialog.UpdatedPilot);
                e.Confirmed = true;
            }
            else
            {
                e.Confirmed = false;
            }
        }

        private void OnEditCompleted(object? sender, PilotActionEventArgs e)
        {
            if (e.Success)
            {
                UpdateStats();
                RefreshSelectedPilot();
            }
        }

        private void OnDeleteRequested(object? sender, PilotActionEventArgs e)
        {
            var dialog = new ConfirmDialog(
                $"Do you want to delete {e.Pilot.Name}?",
                "This pilot will be permanently removed. Any aircraft assigned to this pilot will be unassigned."
            );
            dialog.Owner = Window.GetWindow(this);

            e.Confirmed = dialog.ShowDialog() == true;
        }

        private void OnDeleteCompleted(object? sender, PilotActionEventArgs e)
        {
            if (e.Success)
            {
                UpdateStats();
                UpdatePilotList();
                HideDetails();
                _selectedPilot = null;
            }
        }

        private void OnCreateRequested(object? sender, EventArgs e)
        {
            var dialog = new EditPilotDialog(_logger, _settingsService);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.SaveSuccessful && dialog.UpdatedPilot != null)
            {
                _viewModel.ExecuteCreate(dialog.UpdatedPilot);
            }
        }

        private void OnCreateCompleted(object? sender, PilotActionEventArgs e)
        {
            if (e.Success)
            {
                UpdateStats();
                UpdatePilotList();
            }
        }
    }
}
