using System.Collections.Generic;
using Ace.App.Models;
using Ace.App.Services;

namespace Ace.App.Interfaces
{
    public interface IRouteSuggestionService
    {
        List<SuggestedRoute> GetSuggestedRoutes(Aircraft aircraft, string? lastArrivalIcao = null);
    }
}
