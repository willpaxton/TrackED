using Microsoft.EntityFrameworkCore;
using System;

namespace TrackEd.Models.Entities
{
    [PrimaryKey(nameof(Enumber))]
    public class User
    {
        public int Enumber { get; set; }
        public string EtsuEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int PasswordHash { get; set; }
    }
}
