using Ace.App.Converters;
using Ace.App.Infrastructure;
using Ace.App.Interfaces;
using Ace.App.ViewModels;
using Ace.App.Views.Dialogs;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ace.App.Views.Aircraft
{
    public partial class MarketView : UserControl
    {
        private readonly MarketViewModel _viewModel;
        private readonly ILoggingService _logger;
        private readonly IAircraftImageService _imageService;
        private readonly IAircraftCatalogRepository _catalogRepository;
        private MarketAircraftViewModel? _selectedAircraft;
        private bool _listExpanded = true;
        private bool _isLoaded = false;

        public MarketView(MarketViewModel viewModel, ILoggingService logger, IAircraftImageService imageService, IAircraftCatalogRepository catalogRepository)
        {
            InitializeComponent();
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _catalogRepository = catalogRepository ?? throw new ArgumentNullException(nameof(catalogRepository));
            DataContext = _viewModel;

            _logger.Debug("MarketView: Constructor");

            Loaded += MarketView_Loaded;
        }

        private void MarketView_Loaded(object sender, RoutedEventArgs e)
        {
            _logger.Debug("MarketView: Loaded event");
            _viewModel.LoadMarketAircraft();
            if (CmbSort?.SelectedIndex >= 0)
                _viewModel.SortAircraft(CmbSort.SelectedIndex);
            UpdateStats();
            _isLoaded = true;
            _logger.Debug($"MarketView: Loaded complete - _isLoaded={_isLoaded}");

            TxtFilter.Focus();
        }

        private void SelectAircraft(MarketAircraftViewModel aircraft)
        {
            _selectedAircraft = aircraft;
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
            if (_selectedAircraft == null) return;

            TxtDetailName.Text = _selectedAircraft.DisplayName;
            TxtDetailManufacturer.Text = _selectedAircraft.Manufacturer;
            DetailOldtimerBadge.Visibility = _selectedAircraft.IsOldtimer ? Visibility.Visible : Visibility.Collapsed;
            TxtDetailType.Text = string.IsNullOrEmpty(_selectedAircraft.Type) ? "—" : _selectedAircraft.Type;
            TxtDetailCategory.Text = string.IsNullOrEmpty(_selectedAircraft.Category) ? "—" : _selectedAircraft.Category;
            TxtDetailSize.Text = _selectedAircraft.SizeCategory;
            TxtDetailPassengers.Text = _selectedAircraft.CapacityInfo;
            TxtDetailCargo.Text = _selectedAircraft.CargoInfo;
            TxtDetailCrew.Text = _selectedAircraft.CrewCount > 0 ? $"{_selectedAircraft.CrewCount}" : "—";
            TxtDetailSpeed.Text = _selectedAircraft.CruiseSpeedKts > 0 ? $"{_selectedAircraft.CruiseSpeedKts:N0} kts" : "—";
            TxtDetailRange.Text = _selectedAircraft.MaxRangeNM > 0 ? $"{_selectedAircraft.MaxRangeNM:N0} NM" : "—";
            TxtDetailFuel.Text = _selectedAircraft.FuelCapacityGal > 0 ? $"{_selectedAircraft.FuelCapacityGal:N0} gal" : "—";
            TxtDetailFuelBurn.Text = _selectedAircraft.FuelBurnGalPerHour > 0 ? $"{_selectedAircraft.FuelBurnGalPerHour:F1} gal/h" : "—";
            TxtDetailServiceCeiling.Text = _selectedAircraft.ServiceCeilingFt > 0 ? $"{_selectedAircraft.ServiceCeilingFt:N0} ft" : "—";
            TxtDetailOperatingCost.Text = _selectedAircraft.HourlyOperatingCost > 0 ? $"{_selectedAircraft.HourlyOperatingCost:N0} €/h" : "—";
            TxtDetailPrice.Text = _selectedAircraft.FormattedPrice;
            TxtDetailProfitPerHour.Text = _selectedAircraft.ProfitPerHour > 0 ? $"{_selectedAircraft.ProfitPerHour:N0} €" : "—";

            UpdateAircraftIcon();

            BtnPurchase.IsEnabled = _selectedAircraft.CanAfford;

            if (_selectedAircraft.CanAfford)
            {
                TxtAffordStatus.Text = "✓ You can afford this";
                TxtAffordStatus.Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80));
            }
            else
            {
                TxtAffordStatus.Text = "✗ Not enough balance";
                TxtAffordStatus.Foreground = new SolidColorBrush(Color.FromRgb(244, 67, 54));
            }
        }

        private void UpdateAircraftIcon()
        {
            if (_selectedAircraft == null) return;

            var imagePath = _imageService.GetImagePath(_selectedAircraft.CustomImagePath, _selectedAircraft.SizeCategory, _selectedAircraft.Title);

            if (File.Exists(imagePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    DetailAircraftImage.Source = bitmap;
                    DetailAircraftImage.Visibility = Visibility.Visible;
                    DetailAircraftIcon.Visibility = Visibility.Collapsed;
                    return;
                }
                catch (Exception ex)
                {
                    _logger.Debug($"MarketView: Failed to load aircraft image: {ex.Message}");
                }
            }

            DetailAircraftImage.Source = null;
            DetailAircraftImage.Visibility = Visibility.Collapsed;

            var iconConverter = new SizeToAircraftIconConverter();
            var brushConverter = new SizeToBrushConverter();

            var geometry = iconConverter.Convert(_selectedAircraft.SizeCategory, typeof(Geometry), string.Empty, CultureInfo.CurrentCulture) as Geometry;
            var brush = brushConverter.Convert(_selectedAircraft.SizeCategory, typeof(Brush), string.Empty, CultureInfo.CurrentCulture) as Brush;

            DetailAircraftIcon.Data = geometry;
            DetailAircraftIcon.Fill = brush;
            DetailAircraftIcon.Visibility = Visibility.Visible;
        }

        private void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            var dialog = new SelectAircraftImageDialog(
                _selectedAircraft.DisplayName,
                _selectedAircraft.DisplayName,
                _selectedAircraft.SizeCategory,
                _selectedAircraft.CustomImagePath,
                _imageService,
                _logger);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.ImageChanged)
            {
                try
                {
                    _catalogRepository.UpdateCustomImagePath(_selectedAircraft.Title, dialog.NewImagePath);
                    _viewModel.LoadMarketAircraft();
                    RefreshSelectedAircraft();
                }
                catch (Exception ex)
                {
                    _logger.Error($"MarketView: Error saving custom image: {ex.Message}");
                    InfoDialog.Show("Error", $"Failed to save image: {ex.Message}", Window.GetWindow(this));
                }
            }
        }

        private void UpdateStats()
        {
            if (_viewModel == null) return;

            var count = _viewModel.FilteredAircraft.Count;
            var balance = _viewModel.BalanceText;

            TxtAircraftCount.Text = $"{count} Aircraft";
            TxtBalance.Text = $"Balance: {balance}";
            TxtAircraftListHeader.Text = $"Available Aircraft ({count})";
        }


        private void OnAircraftListItemClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is MarketAircraftViewModel aircraft)
            {
                SelectAircraft(aircraft);
            }
        }

        private void AircraftListHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _listExpanded = !_listExpanded;
            AircraftListScrollViewer.Visibility = _listExpanded ? Visibility.Visible : Visibility.Collapsed;
            TxtExpandIcon.Text = _listExpanded ? "▼" : "▶";
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtFilterHint.Visibility = string.IsNullOrEmpty(TxtFilter.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            if (_isLoaded)
            {
                UpdateStats();
            }
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbSort?.SelectedIndex >= 0 && _viewModel != null && _viewModel.MarketAircraft.Count > 0)
            {
                _viewModel.SortAircraft(CmbSort.SelectedIndex);
            }
        }

        private void CreateAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateMarketAircraftDialog(_logger, _catalogRepository);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.Created)
            {
                _viewModel.LoadMarketAircraft();
                UpdateStats();
            }
        }

        private void EditSpecsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            var dialog = new EditMarketAircraftSpecsDialog(
                _selectedAircraft.Title,
                _selectedAircraft.PassengerCapacity,
                _selectedAircraft.MaxCargoKg,
                _selectedAircraft.CruiseSpeedKts,
                _selectedAircraft.MaxRangeNM,
                _selectedAircraft.FuelCapacityGal,
                _selectedAircraft.FuelBurnGalPerHour,
                _selectedAircraft.ServiceCeilingFt,
                _selectedAircraft.Price,
                _logger,
                _catalogRepository);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true && dialog.Saved)
            {
                _viewModel.LoadMarketAircraft();
                UpdateStats();
                RefreshSelectedAircraft();
            }
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            if (!_selectedAircraft.CanAfford)
                return;

            var owner = Window.GetWindow(this);

            var confirmed = ConfirmDialog.Show(
                "Purchase Aircraft",
                $"Do you want to purchase {_selectedAircraft.DisplayName} for {_selectedAircraft.FormattedPrice}?",
                owner);

            if (!confirmed)
                return;

            var (success, message) = _viewModel.PurchaseAircraft(_selectedAircraft);

            if (success)
            {
                _viewModel.UpdateBalance();
                HideDetails();
                _selectedAircraft = null;
                UpdateStats();
            }
            else
            {
                InfoDialog.Show(
                    "Error",
                    $"Purchase error: {message}",
                    owner);
            }
        }

        private void DeleteAircraftButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAircraft == null) return;

            var owner = Window.GetWindow(this);

            var confirmed = ConfirmDialog.Show(
                "Delete Aircraft",
                $"Do you want to permanently delete '{_selectedAircraft.DisplayName}' from the market?\n\nThis will remove it from the catalog.",
                owner);

            if (!confirmed)
                return;

            try
            {
                _catalogRepository.DeleteByTitle(_selectedAircraft.Title);
                _viewModel.LoadMarketAircraft();

                InfoDialog.Show(
                    "Aircraft Deleted",
                    $"'{_selectedAircraft.DisplayName}' has been removed from the market.",
                    owner);

                HideDetails();
                _selectedAircraft = null;
                UpdateStats();
            }
            catch (Exception ex)
            {
                _logger.Error($"MarketView: Failed to delete aircraft: {ex.Message}");

                InfoDialog.Show(
                    "Error",
                    $"Failed to delete aircraft: {ex.Message}",
                    owner);
            }
        }

        private void RefreshSelectedAircraft()
        {
            if (_selectedAircraft == null) return;

            var refreshed = _viewModel.FilteredAircraft.FirstOrDefault(a => a.Title == _selectedAircraft.Title);
            if (refreshed != null)
            {
                _selectedAircraft = refreshed;
                UpdateDetailsPanel();
            }
            else
            {
                HideDetails();
                _selectedAircraft = null;
            }
        }
    }
}
