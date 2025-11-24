using Microsoft.EntityFrameworkCore;
using System;

namespace TrackEd.Models.Entities
{
    [PrimaryKey(nameof(AssignmentID))]
    public class Assignment
    {
        public int AssignmentID { get; set; }

        public int StudentEnumber { get; set; }   // FK → Student.Enumber
        public int ProfessorEnumber { get; set; } // FK → Professor.Enumber
        public int LocationID { get; set; }       // FK → Location.LocationID

        public DateOnly Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        // Non nullable forign keys required. 
        public Student? Student { get; set; } = null!;
        public Professor? Professor { get; set; } = null!;
        public Location? Location { get; set; } = null!;

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
        // ^ One assignment is tied to many Incident
    }
}
