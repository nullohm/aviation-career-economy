using Ace.App.Interfaces;
using Ace.App.Models;
using Ace.App.Services;
using System;
using System.Windows;

namespace Ace.App.Views.Dialogs
{
    public partial class EditPilotDialog : Window
    {
        private readonly ILoggingService _log;
        private readonly ISettingsService _settingsService;
        private readonly Pilot? _pilot;
        private readonly bool _isNewPilot;

        public bool SaveSuccessful { get; private set; }
        public Pilot? UpdatedPilot { get; private set; }

        public EditPilotDialog(ILoggingService logger, ISettingsService settingsService, Pilot? pilot = null)
        {
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            InitializeComponent();

            _pilot = pilot;
            _isNewPilot = pilot == null;

            if (_isNewPilot)
            {
                TitleText.Text = "Create New Pilot";
                RankBadge.Rank = PilotRankType.Junior;
                TxtName.Text = "New Pilot";
                DateBirthday.SelectedDate = DateTime.Today.AddYears(-25);
                TxtFlightHours.Text = "0";
                TxtDistanceNM.Text = "0";
                ChkIsEmployed.IsChecked = false;

                _log.Info("EditPilotDialog: Opened for creating new pilot");
            }
            else
            {
                var settings = _settingsService.CurrentSettings;
                var rank = PilotRank.GetRank(pilot!.TotalFlightHours, settings);

                TitleText.Text = "Edit Pilot";
                RankBadge.Rank = rank;
                RankBadge.ToolTip = PilotRank.GetRankName(rank);
                TxtName.Text = pilot.Name;
                DateBirthday.SelectedDate = pilot.Birthday;
                TxtFlightHours.Text = pilot.TotalFlightHours.ToString("F0");
                TxtDistanceNM.Text = pilot.TotalDistanceNM.ToString("F0");
                ChkIsEmployed.IsChecked = pilot.IsEmployed;

                _log.Info($"EditPilotDialog: Opened for editing {pilot.Name}");
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                InfoDialog.Show("Invalid Name", "Please enter a valid pilot name.", this);
                return;
            }

            if (!DateBirthday.SelectedDate.HasValue)
            {
                InfoDialog.Show("Invalid Birthday", "Please select a birthday.", this);
                return;
            }

            if (!double.TryParse(TxtFlightHours.Text.Replace(".", "").Replace(",", ""), out var flightHours) || flightHours < 0)
            {
                InfoDialog.Show("Invalid Flight Hours", "Please enter a valid positive number for flight hours.", this);
                return;
            }

            if (!double.TryParse(TxtDistanceNM.Text.Replace(".", "").Replace(",", ""), out var distanceNM) || distanceNM < 0)
            {
                InfoDialog.Show("Invalid Distance", "Please enter a valid positive number for distance.", this);
                return;
            }

            try
            {
                var baseSalary = _settingsService.CurrentSettings.PilotBaseSalary;

                if (_isNewPilot)
                {
                    UpdatedPilot = new Pilot
                    {
                        Name = TxtName.Text.Trim(),
                        Birthday = DateBirthday.SelectedDate.Value,
                        TotalFlightHours = flightHours,
                        TotalDistanceNM = distanceNM,
                        SalaryPerMonth = baseSalary,
                        IsEmployed = ChkIsEmployed.IsChecked ?? false,
                        IsPlayer = false
                    };

                    _log.Info($"EditPilotDialog: Created new pilot {UpdatedPilot.Name}");
                }
                else
                {
                    _pilot!.Name = TxtName.Text.Trim();
                    _pilot.Birthday = DateBirthday.SelectedDate.Value;
                    _pilot.TotalFlightHours = flightHours;
                    _pilot.TotalDistanceNM = distanceNM;
                    _pilot.IsEmployed = ChkIsEmployed.IsChecked ?? false;

                    UpdatedPilot = _pilot;

                    _log.Info($"EditPilotDialog: Updated pilot {_pilot.Name}");
                }

                SaveSuccessful = true;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _log.Error($"EditPilotDialog: Failed to save pilot: {ex.Message}");
                InfoDialog.Show("Error", $"Failed to save pilot: {ex.Message}", this);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _log.Debug("EditPilotDialog: Cancelled by user");
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _log.Debug("EditPilotDialog: Closed via X button");
            DialogResult = false;
            Close();
        }
    }
}
