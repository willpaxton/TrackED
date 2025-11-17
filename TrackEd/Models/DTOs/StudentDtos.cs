namespace TrackEd.Models.DTOs
{
    public class StudentReadDto
    {
        public int Enumber { get; set; }
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int Year { get; set; }
        public string Major { get; set; } = string.Empty;
        public bool IsLocationTrackingOn { get; set; }

        public double? LastLatitude { get; set; }
        public double? LastLongitude { get; set; }
    }

    public class StudentCreateDto
    {
        public int Enumber { get; set; }     // ETSU E-number
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int Year { get; set; }
        public string Major { get; set; } = string.Empty;

        // Optional at creation time, need to True when tracking starts
        public bool IsLocationTrackingOn { get; set; } = false;
        public double? LastLatitude { get; set; }
        public double? LastLongitude { get; set; }
    }

    public class StudentUpdateDto
    {
        public string PhoneNumber { get; set; } = string.Empty;

        public int Year { get; set; }
        public string Major { get; set; } = string.Empty;

        public bool IsLocationTrackingOn { get; set; }

        public double? LastLatitude { get; set; }
        public double? LastLongitude { get; set; }
    }

    public class StudentLocationUpdateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}