using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackEd.Models.Entities;

namespace TrackEd.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> AppUsers => Set<User>(); // Avoids naming conflicts between IdentityDBContext Users and our Application Users
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Professor> Professors => Set<Professor>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Incident> Incidents => Set<Incident>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Inheritance TPH
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Student>("Student")
                .HasValue<Professor>("Professor");

            modelBuilder.Entity<User>()
                .HasKey(u => u.Enumber);

            // Student → Assignments (1-to-many)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Assignments)  
                .HasForeignKey(a => a.StudentEnumber)
                .OnDelete(DeleteBehavior.Restrict);

            // Professor → Assignments (1-to-many)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Professor)
                .WithMany(p => p.ProfessorAssignments)
                .HasForeignKey(a => a.ProfessorEnumber)
                .OnDelete(DeleteBehavior.Restrict);

            // Location → Assignments (1-to-many)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Location)
                .WithMany(l => l.Assignments)
                .HasForeignKey(a => a.LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment → Incidents (1-to-many)
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Assignment)
                .WithMany(a => a.Incidents)
                .HasForeignKey(i => i.AssignmentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Store IncidentReason enum as string
            modelBuilder.Entity<Incident>()
                .Property(i => i.Reason)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}