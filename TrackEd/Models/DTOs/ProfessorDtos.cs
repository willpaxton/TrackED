namespace TrackEd.Models.DTOs
{
    public class ProfessorReadDto
    {
        public int Enumber { get; set; }
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
    }

    public class ProfessorCreateDto
    {
        public int Enumber { get; set; }     // ETSU E-number
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // default false unless you explicitly mark them as admin
        public bool IsAdmin { get; set; } = false;
    }

    public class ProfessorUpdateDto
    {
        // things that might change over time
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}