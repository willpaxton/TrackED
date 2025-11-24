namespace TrackEd.Models.DTOs
{
    public class IncidentCreateDto
    {
        public int AssignmentID { get; set; }
        
        // Optional; defaults to UtcNow if omitted
        public DateTime? Timestamp { get; set; }

        // Send enum name as string (e.g., "Absent", "OutOfRadius")
        public string Reason { get; set; } = "Error";
    }

    public class IncidentDto
    {
        public int IncidentID { get; set; }
        public int AssignmentID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Reason { get; set; } = "";
    }
}