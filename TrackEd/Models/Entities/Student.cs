using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace TrackEd.Models.Entities
{
    public class Student : User

    {
        public int Year { get; set; }
        public string Major { get; set; } = string.Empty;
        public bool IsLocationTrackingOn { get; set; }
        public double? LastLatitude { get; set; } // ? means optional so we dont need cords to create them. 
        public double? LastLongitude { get; set; } // Wont be filled until we actually get their location
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        // ^ One student to many Assignment = Assignments
    }
}
