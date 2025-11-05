using Microsoft.EntityFrameworkCore;
using System;

namespace TrackEd.Models.Entities
{
    [PrimaryKey(nameof(AssignmentID))]
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int LocationID { get; set; }
        public int StartTime { get; set; }

    }
}
