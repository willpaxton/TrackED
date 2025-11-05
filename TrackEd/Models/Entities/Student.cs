using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace TrackEd.Models.Entities
{
    public class Student : User

    {
        public int Year { get; set; }
        public string Major { get; set; }
        public bool IsLocationTrackingOn { get; set; }
        public ICollection<Assignment> Schedule { get; } = new List<Assignment>();

    }
}
