using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILoggingService _logger;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IMaintenanceService _maintenanceService;
        private readonly ILoanRepository _loanRepository;
        private readonly IFBORepository _fboRepository;
        private readonly IPilotRepository _pilotRepository;
        private readonly IAircraftPilotAssignmentRepository _assignmentRepository;
        private readonly List<Notification> _notifications = new();

        public event Action? NotificationsChanged;

        public NotificationService(
            ILoggingService logger,
            IAircraftRepository aircraftRepository,
            IMaintenanceService maintenanceService,
            ILoanRepository loanRepository,
            IFBORepository fboRepository,
            IPilotRepository pilotRepository,
            IAircraftPilotAssignmentRepository assignmentRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _aircraftRepository = aircraftRepository ?? throw new ArgumentNullException(nameof(aircraftRepository));
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));
            _loanRepository = loanRepository ?? throw new ArgumentNullException(nameof(loanRepository));
            _fboRepository = fboRepository ?? throw new ArgumentNullException(nameof(fboRepository));
            _pilotRepository = pilotRepository ?? throw new ArgumentNullException(nameof(pilotRepository));
            _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        }

        public List<Notification> GetAllNotifications()
        {
            return _notifications.OrderByDescending(n => n.Type).ThenBy(n => n.CreatedAt).ToList();
        }

        public List<Notification> GetNotificationsByCategory(NotificationCategory category)
        {
            return _notifications.Where(n => n.Category == category).ToList();
        }

        public int GetNotificationCount()
        {
            return _notifications.Count;
        }

        public int GetWarningCount()
        {
            return _notifications.Count(n => n.Type == NotificationType.Warning);
        }

        public int GetDangerCount()
        {
            return _notifications.Count(n => n.Type == NotificationType.Danger);
        }

        public void RefreshNotifications()
        {
            _notifications.Clear();

            CheckMaintenanceNotifications();
            CheckLoanNotifications();
            CheckOperationsNotifications();

            _logger.Debug($"NotificationService: Refreshed notifications, count={_notifications.Count}");
            NotificationsChanged?.Invoke();
        }

        private void CheckMaintenanceNotifications()
        {
            var aircraft = _aircraftRepository.GetAllAircraft();

            foreach (var ac in aircraft)
            {
                if (ac.Status == AircraftStatus.Maintenance)
                    continue;

                var urgentCheck = _maintenanceService.GetMostUrgentCheck(ac);
                if (urgentCheck == null)
                    continue;

                if (urgentCheck.IsOverdue)
                {
                    _notifications.Add(new Notification
                    {
                        Type = NotificationType.Danger,
                        Category = NotificationCategory.Maintenance,
                        Title = "Wartung überfällig",
                        Message = $"{ac.Registration}: {urgentCheck.Name} ist überfällig",
                        Icon = "\uE7BA",
                        RelatedEntityId = ac.Id
                    });
                }
                else if (urgentCheck.UrgencyScore <= 1)
                {
                    var timeInfo = urgentCheck.HoursRemaining.HasValue
                        ? $"{urgentCheck.HoursRemaining:F0}h verbleibend"
                        : urgentCheck.DaysRemaining.HasValue
                            ? $"{urgentCheck.DaysRemaining} Tage verbleibend"
                            : "bald fällig";

                    _notifications.Add(new Notification
                    {
                        Type = NotificationType.Warning,
                        Category = NotificationCategory.Maintenance,
                        Title = "Wartung fällig",
                        Message = $"{ac.Registration}: {urgentCheck.Name} ({timeInfo})",
                        Icon = "\uE7BA",
                        RelatedEntityId = ac.Id
                    });
                }
            }
        }

        private void CheckLoanNotifications()
        {
            var activeLoans = _loanRepository.GetActiveLoans();

            foreach (var loan in activeLoans)
            {
                var daysSinceTaken = (DateTime.Today - loan.TakenDate).Days;
                var monthsSinceTaken = daysSinceTaken / 30;
                var nextPaymentDay = loan.TakenDate.AddMonths(monthsSinceTaken + 1).Day;
                var daysUntilPayment = (loan.TakenDate.AddMonths(monthsSinceTaken + 1) - DateTime.Today).Days;

                if (daysUntilPayment <= 0)
                {
                    _notifications.Add(new Notification
                    {
                        Type = NotificationType.Danger,
                        Category = NotificationCategory.Loan,
                        Title = "Kreditrate fällig",
                        Message = $"Kredit #{loan.Id}: Rate von {loan.TotalRepayment / 12:N0} € ist heute fällig",
                        Icon = "\uE825",
                        RelatedEntityId = loan.Id
                    });
                }
                else if (daysUntilPayment <= 7)
                {
                    _notifications.Add(new Notification
                    {
                        Type = NotificationType.Warning,
                        Category = NotificationCategory.Loan,
                        Title = "Kreditrate bald fällig",
                        Message = $"Kredit #{loan.Id}: Rate in {daysUntilPayment} Tagen fällig ({loan.TotalRepayment / 12:N0} €)",
                        Icon = "\uE825",
                        RelatedEntityId = loan.Id
                    });
                }
            }
        }

        private void CheckOperationsNotifications()
        {
            var aircraft = _aircraftRepository.GetAllAircraft();
            var fbos = _fboRepository.GetAllFBOs();
            var pilots = _pilotRepository.GetEmployedPilots().Where(p => !p.IsPlayer).ToList();

            var fbosWithoutAircraft = fbos.Where(f => !aircraft.Any(a => a.AssignedFBOId == f.Id && a.Status != AircraftStatus.Maintenance)).ToList();
            var aircraftWithoutFBO = aircraft.Where(a => !a.AssignedFBOId.HasValue && a.Status != AircraftStatus.Maintenance).ToList();
            var assignedPilotIds = _assignmentRepository.GetAllAssignments().Select(a => a.PilotId).ToHashSet();
            var pilotsWithoutAircraft = pilots.Where(p => !assignedPilotIds.Contains(p.Id)).ToList();

            if (fbosWithoutAircraft.Count > 0)
            {
                _notifications.Add(new Notification
                {
                    Type = NotificationType.Warning,
                    Category = NotificationCategory.FBO,
                    Title = "FBOs ohne Aircraft",
                    Message = $"{fbosWithoutAircraft.Count} FBO(s) haben kein zugewiesenes Flugzeug und generieren keine Einnahmen",
                    Icon = "\uE80F"
                });
            }

            if (aircraftWithoutFBO.Count > 0)
            {
                _notifications.Add(new Notification
                {
                    Type = NotificationType.Danger,
                    Category = NotificationCategory.Fleet,
                    Title = "Aircraft ohne FBO",
                    Message = $"{aircraftWithoutFBO.Count} Flugzeug(e) ohne FBO-Zuweisung: {string.Join(", ", aircraftWithoutFBO.Take(3).Select(a => a.Registration))}{(aircraftWithoutFBO.Count > 3 ? "..." : "")}",
                    Icon = "\uE709"
                });
            }

            if (pilotsWithoutAircraft.Count > 0)
            {
                _notifications.Add(new Notification
                {
                    Type = NotificationType.Info,
                    Category = NotificationCategory.Pilot,
                    Title = "Piloten ohne Aircraft",
                    Message = $"{pilotsWithoutAircraft.Count} Pilot(en) ohne Flugzeug: {string.Join(", ", pilotsWithoutAircraft.Take(3).Select(p => p.Name))}{(pilotsWithoutAircraft.Count > 3 ? "..." : "")}",
                    Icon = "\uE77B"
                });
            }
        }
    }
}
