using System.ComponentModel.DataAnnotations;

public record LocationReadDto(
    int LocationID,
    string LocationName,
    double Latitude,
    double Longitude,
    double RadiusMeters);

public class LocationCreateDto {
    [Required, MinLength(2)] public string LocationName { get; set; } = "";
    [Range(-90, 90)]        public double Latitude { get; set; }
    [Range(-180, 180)]      public double Longitude { get; set; }
    [Range(1, 50_000)]      public double RadiusMeters { get; set; } = 50;
}

public class LocationUpdateDto : LocationCreateDto { }