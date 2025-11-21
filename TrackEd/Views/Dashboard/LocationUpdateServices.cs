using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TrackEd.Services
{
    public class LocationUpdateService
    {
        // Store the latest coordinates
        private static double? _latitude;
        private static double? _longitude;
        private static DateTime _lastUpdate = DateTime.MinValue;

        public static void UpdateCoordinates(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
            _lastUpdate = DateTime.Now;
        }

        public static (double? lat, double? lng, DateTime lastUpdate) GetCoordinates()
        {
            return (_latitude, _longitude, _lastUpdate);
        }

        public static string GetMapUrl(bool includeMarker = true)
        {
            if (_latitude == null || _longitude == null)
            {
                return "https://www.google.com/maps/embed/v1/view?key=AIzaSyBpSX02zBnR-ueioYxcK67SF3DIpyo4hWE&center=36.302360811187214,-82.36984874754702&zoom=15";
            }

            string baseUrl = $"https://www.google.com/maps/embed/v1/view?key=AIzaSyBpSX02zBnR-ueioYxcK67SF3DIpyo4hWE&center={_latitude},{_longitude}&zoom=18";
            
            if (includeMarker)
            {
                // Add marker by switching to place mode with coordinates
                return $"https://www.google.com/maps/embed/v1/place?key=AIzaSyBpSX02zBnR-ueioYxcK67SF3DIpyo4hWE&q={_latitude},{_longitude}&zoom=18";
            }

            return baseUrl;
        }

        // Calculate distance between two points using Haversine formula
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth's radius in meters
            
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);
            
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                      Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                      Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            
            return R * c; // Distance in meters
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // Check if student is within acceptable range of a location
        public static bool IsWithinRange(double studentLat, double studentLon, 
                                        double targetLat, double targetLon, 
                                        double maxDistanceMeters = 100)
        {
            double distance = CalculateDistance(studentLat, studentLon, targetLat, targetLon);
            return distance <= maxDistanceMeters;
        }

        // Get status message for student location
        public static string GetLocationStatus(double studentLat, double studentLon,
                                              double targetLat, double targetLon,
                                              double maxDistanceMeters = 100)
        {
            double distance = CalculateDistance(studentLat, studentLon, targetLat, targetLon);
            
            if (distance <= maxDistanceMeters)
            {
                return $"✓ At Location ({distance:F1}m away)";
            }
            else
            {
                return $"✗ Not at Location ({distance:F1}m away)";
            }
        }
    }

    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    // Add a class to define clinical sites
    public class ClinicalSite
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AcceptableRangeMeters { get; set; } = 100; // Default 100 meters
    }
}