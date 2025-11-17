using Microsoft.EntityFrameworkCore;

namespace TrackEd.Models.Entities
{
    public class Professor : User
    {
        public bool IsAdmin { get; set; }
        public ICollection<Assignment> ProfessorAssignments { get; set; } = new List<Assignment>();
        // ^ One professor many assignment = ProfessorAssignments 
    }
}