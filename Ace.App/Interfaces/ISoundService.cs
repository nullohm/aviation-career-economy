namespace Ace.App.Interfaces
{
    public enum SoundType
    {
        FlightCompleted,
        AchievementUnlocked,
        Warning,
        Notification,
        ButtonClick,
        TopOfDescent,
        StallWarning
    }

    public interface ISoundService
    {
        bool IsSoundEnabled { get; set; }
        double Volume { get; set; }

        void Play(SoundType soundType);
        void PlayFlightCompleted();
        void PlayAchievementUnlocked();
        void PlayWarning();
        void PlayNotification();
        void PlayTopOfDescent();
        void PlayStallWarning();
    }
}
