using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Ace.App.Interfaces;

namespace Ace.App.Views.Dialogs
{
    public partial class SelectAircraftImageDialog : Window
    {
        private readonly IAircraftImageService _imageService;
        private readonly ILoggingService _log;
        private readonly string _identifier;
        private readonly string _sizeCategory;
        private readonly string? _currentCustomPath;

        private string? _selectedImagePath;
        private bool _resetToDefault;

        public bool ImageChanged { get; private set; }
        public string? NewImagePath { get; private set; }

        public SelectAircraftImageDialog(
            string aircraftName,
            string identifier,
            string sizeCategory,
            string? currentCustomPath,
            IAircraftImageService imageService,
            ILoggingService logger)
        {
            InitializeComponent();

            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _identifier = identifier;
            _sizeCategory = sizeCategory;
            _currentCustomPath = currentCustomPath;

            TxtAircraftName.Text = aircraftName;
            LoadCurrentImage();

            _log.Debug($"SelectAircraftImageDialog: Opened for {aircraftName} (ID: {identifier})");
        }

        private void LoadCurrentImage()
        {
            var imagePath = _imageService.GetImagePath(_currentCustomPath, _sizeCategory);
            LoadImagePreview(imagePath);
        }

        private void LoadImagePreview(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview.Source = bitmap;
                    TxtNoImage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ImagePreview.Source = null;
                    TxtNoImage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                _log.Error($"SelectAircraftImageDialog: Failed to load image preview: {ex.Message}");
                ImagePreview.Source = null;
                TxtNoImage.Visibility = Visibility.Visible;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Aircraft Image",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|PNG Files|*.png|JPEG Files|*.jpg;*.jpeg|BMP Files|*.bmp|All Files|*.*",
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedImagePath = dialog.FileName;
                _resetToDefault = false;
                LoadImagePreview(_selectedImagePath);
                _log.Debug($"SelectAircraftImageDialog: Selected image: {_selectedImagePath}");
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _selectedImagePath = null;
            _resetToDefault = true;

            var defaultPath = _imageService.GetImagePath(null, _sizeCategory);
            LoadImagePreview(defaultPath);
            _log.Debug($"SelectAircraftImageDialog: Reset to default image");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_resetToDefault)
                {
                    _imageService.RemoveCustomImage(_identifier);
                    NewImagePath = null;
                    ImageChanged = true;
                    _log.Info($"SelectAircraftImageDialog: Removed custom image for {_identifier}");
                }
                else if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    NewImagePath = _imageService.SetCustomImage(_selectedImagePath, _identifier);
                    ImageChanged = true;
                    _log.Info($"SelectAircraftImageDialog: Set custom image for {_identifier}: {NewImagePath}");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _log.Error($"SelectAircraftImageDialog: Failed to save image: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to save image: {ex.Message}", this);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
