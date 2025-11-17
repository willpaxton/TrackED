using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackEd.Data;
using TrackEd.Models.Entities;

namespace TrackEd.DbSeeding
{
    public static class DbSeeder
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope   = services.CreateScope();
            var context       = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Apply migrations / ensure DB exists
            await context.Database.MigrateAsync();

            // Simple “already seeded?” guard
            if (context.AppUsers.Any())
                return;

            // ----- SEED PROFESSOR -----
            var prof = new Professor
            {
                Enumber      = 9000001,
                EtsuEmail    = "prof1@etsu.edu",
                FirstName    = "Test",
                LastName     = "Professor",
                PhoneNumber  = "555-0001",
                IsAdmin      = true,
                PasswordHash = string.Empty
            };

            // ----- SEED STUDENT -----
            var student = new Student
            {
                Enumber               = 9001001,
                EtsuEmail             = "student1@etsu.edu",
                FirstName             = "Test",
                LastName              = "Student",
                PhoneNumber           = "555-1001",
                Year                  = 3,
                Major                 = "Nursing",
                IsLocationTrackingOn  = false,
                LastLatitude          = null,
                LastLongitude         = null,
                PasswordHash          = string.Empty
            };

            // ----- SEED LOCATION -----
            var loc = new Location
            {
                LocationName = "Nursing Building",
                Latitude     = 36.303,
                Longitude    = -82.369,
                RadiusMeters = 100
            };

            context.AppUsers.Add(prof);
            context.AppUsers.Add(student);
            context.Locations.Add(loc);

            // Save once so LocationID gets generated
            await context.SaveChangesAsync();

            // ----- SEED ASSIGNMENT -----
            // Use DateOnly + simple int times (e.g. 900 = 9:00, 1030 = 10:30)
            var assignment = new Assignment
            {
                StudentEnumber   = student.Enumber,
                ProfessorEnumber = prof.Enumber,
                LocationID       = loc.LocationID,
                Date             = DateOnly.FromDateTime(DateTime.Today),
                StartTime        = 900,   // 9:00
                EndTime          = 1030   // 10:30
            };

            context.Assignments.Add(assignment);
            await context.SaveChangesAsync();
        }
    }
}