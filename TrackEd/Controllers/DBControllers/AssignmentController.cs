using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;
using TrackEd.Models.DTOs;

namespace TrackEd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/assignment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentReadDto>>> GetAll()
        {
            var list = await _context.Assignments
                .Include(a => a.Professor)
                .Include(a => a.Location)
                .AsNoTracking()
                .Select(a => new AssignmentReadDto(
                    a.AssignmentID,
                    a.StudentEnumber,
                    a.ProfessorEnumber,
                    a.LocationID,
                    a.Date,
                    a.StartTime,
                    a.EndTime,
                    a.Professor != null ? $"{a.Professor.FirstName} {a.Professor.LastName}" : null,
                    a.Location != null ? a.Location.LocationName : null
                ))
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/assignment/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AssignmentReadDto>> GetById(int id)
        {
            var a = await _context.Assignments
                .Include(x => x.Professor)
                .Include(x => x.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AssignmentID == id);

            if (a is null) return NotFound($"Assignment {id} not found.");

            var dto = new AssignmentReadDto(
                a.AssignmentID, a.StudentEnumber, a.ProfessorEnumber, a.LocationID,
                a.Date, a.StartTime, a.EndTime,
                ProfessorName: a.Professor != null ? $"{a.Professor.FirstName} {a.Professor.LastName}" : null,
                LocationName: a.Location != null ? a.Location.LocationName : null
            );

            return Ok(dto);
            }
        
        // GET: api/assignment/student/12345
        [HttpGet("student/{enumber:int}")]
        public async Task<ActionResult<IEnumerable<AssignmentReadDto>>> GetForStudent(int enumber)
        {
            var list = await _context.Assignments
                .Where(a => a.StudentEnumber == enumber)
                .Include(a => a.Professor)
                .Include(a => a.Location)
                .AsNoTracking()
                .Select(a => new AssignmentReadDto(
                    a.AssignmentID,
                    a.StudentEnumber,
                    a.ProfessorEnumber,
                    a.LocationID,
                    a.Date,
                    a.StartTime,
                    a.EndTime,
                    a.Professor != null ? $"{a.Professor.FirstName} {a.Professor.LastName}" : null,
                    a.Location != null ? a.Location.LocationName : null
                ))
                .ToListAsync();

            if (list.Count == 0) return NotFound($"No assignments for student {enumber}.");
            return Ok(list);
        }

        // POST: api/assignment
        [HttpPost]
        public async Task<ActionResult<AssignmentReadDto>> Create([FromBody] AssignmentCreateDto dto)
        {
            // Guard rails: ensure FKs exist
            var studentExists   = await _context.AppUsers.OfType<Student>().AnyAsync(s => s.Enumber == dto.StudentEnumber);
            var professorExists = await _context.AppUsers.OfType<Professor>().AnyAsync(p => p.Enumber == dto.ProfessorEnumber);
            var locationExists  = await _context.Locations.AnyAsync(l => l.LocationID == dto.LocationID);

            if (!studentExists)   return BadRequest($"Student {dto.StudentEnumber} does not exist.");
            if (!professorExists) return BadRequest($"Professor {dto.ProfessorEnumber} does not exist.");
            if (!locationExists)  return BadRequest($"Location {dto.LocationID} does not exist.");

            var entity = new Assignment
            {
                StudentEnumber   = dto.StudentEnumber,
                ProfessorEnumber = dto.ProfessorEnumber,
                LocationID       = dto.LocationID,
                Date             = dto.Date,
                StartTime        = dto.StartTime,
                EndTime          = dto.EndTime
            };

            _context.Assignments.Add(entity);
            await _context.SaveChangesAsync();

            // Return the created resource’s read shape
            return CreatedAtAction(nameof(GetById), new { id = entity.AssignmentID }, new AssignmentReadDto(
                entity.AssignmentID, entity.StudentEnumber, entity.ProfessorEnumber, entity.LocationID,
                entity.Date, entity.StartTime, entity.EndTime
            ));
        }

        // PUT: api/assignment/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] AssignmentUpdateDto dto)
        {
            var entity = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentID == id);
            if (entity is null) return NotFound($"Assignment {id} not found.");

            // Optional: re-validate FKs if they changed
            if (entity.StudentEnumber != dto.StudentEnumber &&
                !await _context.AppUsers.OfType<Student>().AnyAsync(s => s.Enumber == dto.StudentEnumber))
                return BadRequest($"Student {dto.StudentEnumber} does not exist.");

            if (entity.ProfessorEnumber != dto.ProfessorEnumber &&
                !await _context.AppUsers.OfType<Professor>().AnyAsync(p => p.Enumber == dto.ProfessorEnumber))
                return BadRequest($"Professor {dto.ProfessorEnumber} does not exist.");

            if (entity.LocationID != dto.LocationID &&
                !await _context.Locations.AnyAsync(l => l.LocationID == dto.LocationID))
                return BadRequest($"Location {dto.LocationID} does not exist.");

            entity.StudentEnumber   = dto.StudentEnumber;
            entity.ProfessorEnumber = dto.ProfessorEnumber;
            entity.LocationID       = dto.LocationID;
            entity.Date             = dto.Date;
            entity.StartTime        = dto.StartTime;
            entity.EndTime          = dto.EndTime;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/assignment/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentID == id);
            if (entity is null) return NotFound($"Assignment {id} not found.");

            _context.Assignments.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}