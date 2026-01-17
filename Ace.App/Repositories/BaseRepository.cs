using System;
using Ace.App.Interfaces;

namespace Ace.App.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ILoggingService _logger;

        protected BaseRepository(ILoggingService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
