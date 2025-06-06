﻿namespace CourierCounter.Location
{

    //Haversine formula
    public class GeoHelper
    {
        public static double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of Earth in km
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double angle) => angle * Math.PI / 180;
    }
}
