using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;
using TrackEd.Models.DTOs;

namespace TrackEd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetAll()
        {
            var students = await _context.Students
                .AsNoTracking()
                .Select(s => new StudentReadDto
                {
                    Enumber = s.Enumber,
                    EtsuEmail = s.EtsuEmail,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    PhoneNumber = s.PhoneNumber,
                    Year = s.Year,
                    Major = s.Major,
                    IsLocationTrackingOn = s.IsLocationTrackingOn,
                    LastLatitude = s.LastLatitude,
                    LastLongitude = s.LastLongitude
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/student/123456
        [HttpGet("{enumber:int}")]
        public async Task<ActionResult<StudentReadDto>> GetByEnumber(int enumber)
        {
            var s = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Enumber == enumber);

            if (s == null)
                return NotFound($"Student {enumber} not found.");

            var dto = new StudentReadDto
            {
                Enumber = s.Enumber,
                EtsuEmail = s.EtsuEmail,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PhoneNumber = s.PhoneNumber,
                Year = s.Year,
                Major = s.Major,
                IsLocationTrackingOn = s.IsLocationTrackingOn,
                LastLatitude = s.LastLatitude,
                LastLongitude = s.LastLongitude
            };

            return Ok(dto);
        }

        // GET: api/student/by-email?email=student@etsu.edu
        // This is the helper your web team can use to turn email -> ENumber
        [HttpGet("by-email")]
        public async Task<ActionResult<StudentReadDto>> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var s = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EtsuEmail == email);

            if (s == null)
                return NotFound($"Student with email {email} not found.");

            var dto = new StudentReadDto
            {
                Enumber = s.Enumber,
                EtsuEmail = s.EtsuEmail,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PhoneNumber = s.PhoneNumber,
                Year = s.Year,
                Major = s.Major,
                IsLocationTrackingOn = s.IsLocationTrackingOn,
                LastLatitude = s.LastLatitude,
                LastLongitude = s.LastLongitude
            };

            return Ok(dto);
        }

        // POST: api/student
        [HttpPost]
        public async Task<ActionResult<StudentReadDto>> Create([FromBody] StudentCreateDto dto)
        {
            // Basic guard rails: unique Enumber + Email across AppUsers
            var enumberExists = await _context.AppUsers
                .AnyAsync(u => u.Enumber == dto.Enumber);
            if (enumberExists)
                return Conflict($"A user with Enumber {dto.Enumber} already exists.");

            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.EtsuEmail == dto.EtsuEmail);
            if (emailExists)
                return Conflict($"A user with email {dto.EtsuEmail} already exists.");

            var student = new Student
            {
                Enumber = dto.Enumber,
                EtsuEmail = dto.EtsuEmail,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Year = dto.Year,
                Major = dto.Major,
                IsLocationTrackingOn = dto.IsLocationTrackingOn,
                LastLatitude = dto.LastLatitude,
                LastLongitude = dto.LastLongitude,
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var readDto = new StudentReadDto
            {
                Enumber = student.Enumber,
                EtsuEmail = student.EtsuEmail,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                Year = student.Year,
                Major = student.Major,
                IsLocationTrackingOn = student.IsLocationTrackingOn,
                LastLatitude = student.LastLatitude,
                LastLongitude = student.LastLongitude
            };

            return CreatedAtAction(nameof(GetByEnumber),
                new { enumber = student.Enumber },
                readDto);
        }

        // PUT: api/student/123456
        // Full update of student profile (except Enumber + Email)
        [HttpPut("{enumber:int}")]
        public async Task<IActionResult> Update(int enumber, [FromBody] StudentUpdateDto dto)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Enumber == enumber);

            if (student == null)
                return NotFound($"Student {enumber} not found.");

            student.PhoneNumber = dto.PhoneNumber;
            student.Year = dto.Year;
            student.Major = dto.Major;
            student.IsLocationTrackingOn = dto.IsLocationTrackingOn;
            student.LastLatitude = dto.LastLatitude;
            student.LastLongitude = dto.LastLongitude;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/student/123456/location
        // Lightweight endpoint for JUST location updates from the mobile/web tracker
        [HttpPut("{enumber:int}/location")]
        public async Task<IActionResult> UpdateLocation(int enumber, [FromBody] StudentLocationUpdateDto dto)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Enumber == enumber);

            if (student == null)
                return NotFound($"Student {enumber} not found.");

            student.LastLatitude = dto.Latitude;
            student.LastLongitude = dto.Longitude;
            student.IsLocationTrackingOn = true; // optional: automatically mark as tracking

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/student/123456
        [HttpDelete("{enumber:int}")]
        public async Task<IActionResult> Delete(int enumber)
        {
            var student = await _context.Students
                .Include(s => s.Assignments)
                .FirstOrDefaultAsync(s => s.Enumber == enumber);

            if (student == null)
                return NotFound($"Student {enumber} not found.");

            // Because of FK Restrict on Assignments, you probably
            // want to prevent deleting students that still have assignments.
            if (student.Assignments.Any())
                return BadRequest("Cannot delete student with existing assignments. Remove or reassign them first.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}