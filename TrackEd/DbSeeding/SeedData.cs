// TrackEd/DbSeeding/SeedData.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackEd.Data;
using TrackEd.Models.Entities;

namespace TrackEd.DbSeeding
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Make sure DB is created & migrated
            await ctx.Database.MigrateAsync();

            // If we already have anything in core tables, bail out (idempotent)
            var hasAny =
                await ctx.Set<Location>().AnyAsync() ||
                await ctx.Set<Assignment>().AnyAsync() ||
                await ctx.Set<Incident>().AnyAsync() ||
                await ctx.Set<User>().AnyAsync(); // whatever your base user type class is named

            if (hasAny) return;

            // --- Seed minimal rows in the correct FK order ---

            // 1) Location
            var jcClinic = new Location
            {
                LocationName = "ETSU Johnson City Clinic",
                Latitude = 36.3137,
                Longitude = -82.3537,
                RadiusMeters = 150
            };
            ctx.Add(jcClinic);
            await ctx.SaveChangesAsync();

            // 2) Users (TPH base = AppUser/User; derived = Student/Professor)
            var prof = new Professor
            {
                EtsuEmail = "prof.miller@etsu.edu",
                FirstName = "Sam",
                LastName = "Miller",
                PhoneNumber = "555-0100",
                IsAdmin = true,
                // any other professor-specific fields if present
            };
            var student = new Student
            {
                EtsuEmail = "tstudent1@etsu.edu",
                FirstName = "Taylor",
                LastName = "Student",
                PhoneNumber = "555-0101",
                Year = 3,
                Major = "IT",
                IsLocationTrackingOn = true
            };
            ctx.AddRange(prof, student);
            await ctx.SaveChangesAsync();

            // 3) Assignment (1 Student → many Assignments; each has a Professor + Location)
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var assignment = new Assignment
            {
                StudentEnumber = student.Enumber,
                ProfessorEnumber = prof.Enumber,
                LocationID = jcClinic.LocationID,
                Date = today,
                StartTime = 900,  // 09:00 (you’re using int; keeping that consistent)
                EndTime = 1100
            };
            ctx.Add(assignment);
            await ctx.SaveChangesAsync();

            // 4) Incident linked to the assignment
            var incident = new Incident
            {
                AssignmentID = assignment.AssignmentID,
                Timestamp = DateTime.UtcNow,
                Reason = IncidentReason.Error // or LateArrival/NoShow/etc.
            };
            ctx.Add(incident);
            await ctx.SaveChangesAsync();
        }
    }
}