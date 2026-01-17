using System;
using System.Collections.Generic;
using System.Linq;
using Ace.App.Data;
using Ace.App.Interfaces;
using Ace.App.Models;

namespace Ace.App.Repositories
{
    public class TypeRatingRepository : BaseRepository, ITypeRatingRepository
    {
        public TypeRatingRepository(ILoggingService logger) : base(logger) { }

        public List<TypeRating> GetTypeRatingsByPilotId(int pilotId)
        {
            try
            {
                using var db = new AceDbContext();
                return db.TypeRatings.Where(t => t.PilotId == pilotId).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"TypeRatingRepository: Failed to get type ratings for pilot {pilotId}: {ex.Message}");
                return new List<TypeRating>();
            }
        }

        public void AddTypeRating(TypeRating typeRating)
        {
            try
            {
                using var db = new AceDbContext();
                db.TypeRatings.Add(typeRating);
                db.SaveChanges();
                _logger.Info($"TypeRatingRepository: Added type rating {typeRating.AircraftType} for pilot {typeRating.PilotId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"TypeRatingRepository: Failed to add type rating: {ex.Message}");
                throw;
            }
        }

        public void DeleteTypeRating(int id)
        {
            try
            {
                using var db = new AceDbContext();
                var typeRating = db.TypeRatings.Find(id);
                if (typeRating != null)
                {
                    db.TypeRatings.Remove(typeRating);
                    db.SaveChanges();
                    _logger.Info($"TypeRatingRepository: Deleted type rating {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"TypeRatingRepository: Failed to delete type rating {id}: {ex.Message}");
                throw;
            }
        }
    }
}
