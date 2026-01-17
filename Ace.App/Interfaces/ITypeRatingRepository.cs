using System.Collections.Generic;
using Ace.App.Models;

namespace Ace.App.Interfaces
{
    public interface ITypeRatingRepository
    {
        List<TypeRating> GetTypeRatingsByPilotId(int pilotId);
        void AddTypeRating(TypeRating typeRating);
        void DeleteTypeRating(int id);
    }
}
