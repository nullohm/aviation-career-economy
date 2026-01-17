using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ace.App.Infrastructure
{
    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public static T GetService<T>() where T : notnull
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceLocator has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        public static IServiceScope CreateScope()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceLocator has not been initialized. Call Initialize() first.");
            }

            return _serviceProvider.CreateScope();
        }
    }
}
