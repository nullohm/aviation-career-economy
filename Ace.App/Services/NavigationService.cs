using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Ace.App.Interfaces;

namespace Ace.App.Services
{


    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggingService _logger;
        private ContentControl? _contentControl;

        public NavigationService(IServiceProvider serviceProvider, ILoggingService logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public void Initialize(ContentControl contentControl)
        {
            _contentControl = contentControl ?? throw new ArgumentNullException(nameof(contentControl));
            _logger.Debug("NavigationService: Initialized with ContentControl");
        }

        public void NavigateTo<TView>() where TView : UserControl
        {
            NavigateTo(typeof(TView));
        }

        public void NavigateTo(Type viewType)
        {
            if (_contentControl == null)
            {
                _logger.Error("NavigationService: Not initialized. Call Initialize() first.");
                throw new InvalidOperationException("NavigationService must be initialized before navigation");
            }

            if (viewType == null)
            {
                _logger.Error("NavigationService: Attempted to navigate to null view type");
                return;
            }

            if (!typeof(UserControl).IsAssignableFrom(viewType))
            {
                _logger.Error($"NavigationService: {viewType.Name} is not a UserControl");
                return;
            }

            try
            {
                _logger.Info($"Navigation: {viewType.Name}");

                var view = _serviceProvider.GetRequiredService(viewType) as UserControl;
                if (view != null)
                {
                    _contentControl.Content = view;
                    _logger.Debug($"NavigationService: Successfully navigated to {viewType.Name}");
                }
                else
                {
                    _logger.Error($"NavigationService: Failed to create view instance for {viewType.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"NavigationService: Error navigating to {viewType.Name}: {ex.Message}");
                throw;
            }
        }
    }
}
