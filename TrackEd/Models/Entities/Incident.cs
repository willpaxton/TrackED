using Microsoft.EntityFrameworkCore;
namespace TrackEd.Models.Entities
{
    public class Incident
    {
        public int IncidentID { get; set; }
        public int AssignmentID { get; set; } // FK
        public Assignment Assignment { get; set; } = null!; // Which Assignment the incident is tied too 
        public DateTime Timestamp { get; set; }
        public IncidentReason Reason { get; set; } // An Enum of Reasons why the incident was created. 
    }
}
