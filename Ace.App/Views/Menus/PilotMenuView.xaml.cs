using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Ace.App.Interfaces;
using Ace.App.Services;

namespace Ace.App.Views.Menus
{
    public partial class PilotMenuView : UserControl
    {
        private Models.Pilot _currentPilot = new Models.Pilot();
        private readonly IPersistenceService _persistenceService;
        private readonly ILoggingService _logger;

        public PilotMenuView(
            IPersistenceService persistenceService,
            ILoggingService logger)
        {
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            InitializeComponent();
            LoadData();

            _persistenceService.PilotProfileChanged += () =>
            {
                 Dispatcher.Invoke(LoadData);
            };
        }

        private void LoadData()
        {
            _currentPilot = _persistenceService.GetActivePilot();

            TxtName.Text = _currentPilot.Name;
            DpBirthday.SelectedDate = _currentPilot.Birthday;

            UpdateImageDisplay(_currentPilot.ImagePath);

            var totalTime = _persistenceService.GetTotalFlightTime();
            TxtTotalTime.Text = $"{(int)totalTime.TotalHours}h {totalTime.Minutes}m";

            LicensesGrid.ItemsSource = _currentPilot.Licenses;
            TypeRatingsGrid.ItemsSource = _currentPilot.TypeRatings;
        }

        private void UpdateImageDisplay(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    var imagePath = path;
                    if (!System.IO.Path.IsPathRooted(path))
                    {
                        imagePath = System.IO.Path.GetFullPath(path);
                    }

                    if (System.IO.File.Exists(imagePath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.EndInit();
                        PilotImage.Source = bitmap;
                        _logger.Debug($"Pilot image loaded: {imagePath}");
                    }
                    else
                    {
                        PilotImage.Source = null;
                        _logger.Warn($"Pilot image not found: {imagePath}");
                    }
                }
                catch (Exception ex)
                {
                    PilotImage.Source = null;
                    _logger.Error($"Failed to load pilot image: {path}", ex);
                }
            }
            else
            {
                PilotImage.Source = null;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPilot == null) return;

            _currentPilot.Name = TxtName.Text;
            _currentPilot.Birthday = DpBirthday.SelectedDate ?? DateTime.Today;

            _persistenceService.SavePilot(_currentPilot);

            _logger.Info($"Pilot profile saved: {_currentPilot.Name}");
        }

        private void BtnChangeImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Bilder (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                _currentPilot.ImagePath = dlg.FileName;
                UpdateImageDisplay(dlg.FileName);
            }
        }
    }
}
