using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Views.Core
{
    public partial class AchievementsView : UserControl
    {
        private readonly IAchievementService _achievementService;
        private List<Achievement> _allAchievements = new();
        private AchievementCategory? _selectedCategory;

        public AchievementsView()
        {
            InitializeComponent();
            _achievementService = App.ServiceProvider.GetRequiredService<IAchievementService>();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAchievements();
        }

        private void LoadAchievements()
        {
            _allAchievements = _achievementService.GetAllAchievements();
            UpdateStats();
            ApplyFilter();
        }

        private void UpdateStats()
        {
            var unlocked = _achievementService.GetTotalUnlocked();
            var total = _achievementService.GetTotalAchievements();
            var percent = _achievementService.GetCompletionPercent();

            TxtProgress.Text = $"{unlocked} / {total} unlocked";
            TxtCompletionPercent.Text = $"{percent:F1}%";
        }

        private void CategoryFilter_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
            {
                _selectedCategory = rb.Name switch
                {
                    "RbFlights" => AchievementCategory.Flights,
                    "RbDistance" => AchievementCategory.Distance,
                    "RbFleet" => AchievementCategory.Fleet,
                    "RbFinance" => AchievementCategory.Finance,
                    "RbFBOs" => AchievementCategory.FBOs,
                    "RbPilots" => AchievementCategory.Pilots,
                    "RbSpecial" => AchievementCategory.Special,
                    _ => null
                };
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            if (AchievementsPanel == null)
                return;

            var filtered = _selectedCategory.HasValue
                ? _allAchievements.Where(a => a.Category == _selectedCategory.Value)
                : _allAchievements;

            var unlocked = filtered.Where(a => a.IsUnlocked).OrderBy(a => a.UnlockedDate);
            var locked = filtered.Where(a => !a.IsUnlocked)
                .OrderBy(a => a.Tier)
                .ThenByDescending(a => a.ProgressPercent);

            var sorted = unlocked.Concat(locked).ToList();

            AchievementsPanel.ItemsSource = sorted;
        }
    }
}
