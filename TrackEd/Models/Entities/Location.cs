using Microsoft.EntityFrameworkCore;
namespace TrackEd.Models.Entities
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusMeters { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        // ^ One location can have many Assignment = Assignments 
    }
}