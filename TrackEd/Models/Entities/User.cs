using Microsoft.EntityFrameworkCore;
using System;

namespace TrackEd.Models.Entities
{
    [PrimaryKey(nameof(Enumber))]
    public abstract class User
    {
        public int Enumber { get; set; }
        public string EtsuEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }  = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
