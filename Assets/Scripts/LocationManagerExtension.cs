using System;

public static class LocationManagerExtension
{
    public static double CalcMoveDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Radius of the Earth in kilometers
        const int R = 6378137;
        
        // Haversine formula
        var dLat = (lat2 - lat1) * (Math.PI / 180);
        var dLon = (lon2 - lon1) * (Math.PI / 180);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = R * c;
        return distance;
    }
}