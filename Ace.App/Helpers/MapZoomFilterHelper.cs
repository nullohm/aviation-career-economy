namespace Ace.App.Helpers
{
    public static class MapZoomFilterHelper
    {
        public static int GetMinRunwayLengthForZoom(double resolution)
        {
            if (resolution > 8000)
            {
                return int.MaxValue;
            }
            else if (resolution > 4000)
            {
                return 10000;
            }
            else if (resolution > 2000)
            {
                return 8000;
            }
            else if (resolution > 1000)
            {
                return 6000;
            }
            else if (resolution > 500)
            {
                return 4000;
            }
            else if (resolution > 200)
            {
                return 2000;
            }
            else
            {
                return 0;
            }
        }

        public static bool ShouldShowNormalAirport(int airportRunwayLength, int minRunwayLength)
        {
            if (minRunwayLength == int.MaxValue)
            {
                return false;
            }

            return airportRunwayLength >= minRunwayLength;
        }
    }
}
