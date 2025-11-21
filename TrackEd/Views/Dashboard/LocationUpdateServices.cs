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
    }

    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}