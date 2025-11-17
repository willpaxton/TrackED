namespace TrackEd.Models.DTOs
{
    public class UserReadDto
    {
        public int Enumber { get; set; }
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // From the EF discriminator: "Student", "Professor", or "User"
        public string UserType { get; set; } = string.Empty;
    }

    public class UserUpdateDto
    {
        // All optional; only send what you want to change
        public string? EtsuEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        // For this project only – NOT secure. Just stored in User.PasswordHash.
        public string? Password { get; set; }
    }
}