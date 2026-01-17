using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Ace.App.Models;

namespace Ace.App.Views.Dialogs
{
    public partial class AchievementNotification : Window
    {
        private readonly DispatcherTimer _closeTimer;

        public AchievementNotification(Achievement achievement)
        {
            InitializeComponent();

            TxtIcon.Text = achievement.Icon;
            TxtTitle.Text = achievement.Title;
            TxtDescription.Text = achievement.Description;

            if (achievement.Reward.HasValue && achievement.Reward.Value > 0)
            {
                TxtReward.Text = $"+€{achievement.Reward.Value:N0}";
                TxtReward.Visibility = Visibility.Visible;
            }

            _closeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(4)
            };
            _closeTimer.Tick += (s, e) =>
            {
                _closeTimer.Stop();
                FadeOut();
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;
            Left = workArea.Right - Width - 20;
            Top = workArea.Bottom - Height - 20;

            Opacity = 0;
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            BeginAnimation(OpacityProperty, fadeIn);

            _closeTimer.Start();
        }

        private void FadeOut()
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) => Close();
            BeginAnimation(OpacityProperty, fadeOut);
        }

        public static void Show(Achievement achievement)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var notification = new AchievementNotification(achievement);
                notification.Show();
            }));
        }
    }
}
