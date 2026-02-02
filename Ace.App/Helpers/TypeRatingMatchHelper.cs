using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace.App.Helpers
{
    public static class TypeRatingMatchHelper
    {
        public static bool HasMatchingTypeRating(string aircraftType, IEnumerable<string> pilotTypeRatings)
        {
            var typeNormalized = aircraftType.Trim();

            foreach (var rating in pilotTypeRatings)
            {
                var ratingNormalized = rating.Trim();

                if (ratingNormalized.Equals(typeNormalized, StringComparison.OrdinalIgnoreCase))
                    return true;

                if (ratingNormalized.Contains(typeNormalized, StringComparison.OrdinalIgnoreCase))
                    return true;

                if (typeNormalized.Contains(ratingNormalized, StringComparison.OrdinalIgnoreCase))
                    return true;

                if (ratingNormalized.EndsWith("Family", StringComparison.OrdinalIgnoreCase))
                {
                    var familyBase = ratingNormalized[..^6].Trim();
                    if (typeNormalized.StartsWith(familyBase, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                if (MatchByTokens(typeNormalized, ratingNormalized))
                    return true;
            }

            return false;
        }

        private static bool MatchByTokens(string typeNormalized, string ratingNormalized)
        {
            var ratingTokens = ratingNormalized.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var typeTokens = typeNormalized.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            var significantMatches = 0;
            foreach (var typeToken in typeTokens)
            {
                var isNumber = int.TryParse(typeToken, out _);
                if (typeToken.Length < 2 || (typeToken.Length < 3 && !isNumber))
                    continue;

                foreach (var ratingToken in ratingTokens)
                {
                    if (ratingToken.Equals(typeToken, StringComparison.OrdinalIgnoreCase) ||
                        ratingToken.StartsWith(typeToken, StringComparison.OrdinalIgnoreCase) ||
                        typeToken.StartsWith(ratingToken, StringComparison.OrdinalIgnoreCase))
                    {
                        significantMatches++;
                        break;
                    }
                }
            }

            return significantMatches >= 2 ||
                   (significantMatches >= 1 &&
                    (typeTokens.Any(t => t.Any(char.IsDigit)) || ratingTokens.Any(t => t.Any(char.IsDigit))));
        }
    }
}
