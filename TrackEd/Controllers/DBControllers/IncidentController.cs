using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;
using TrackEd.Models.DTOs;

namespace TrackEd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncidentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/incident
        // Optional filter: ?assignmentId=123
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentDto>>> GetIncidents([FromQuery] int? assignmentId)
        {
            IQueryable<Incident> query = _context.Incidents.AsNoTracking();

            if (assignmentId.HasValue)
                query = query.Where(i => i.AssignmentID == assignmentId.Value);

            var data = await query
                .OrderByDescending(i => i.Timestamp)
                .Select(i => new IncidentDto
                {
                    IncidentID = i.IncidentID,
                    AssignmentID = i.AssignmentID,
                    Timestamp = i.Timestamp,
                    Reason = i.Reason.ToString()
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/incident/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IncidentDto>> GetIncident(int id)
        {
            var i = await _context.Incidents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IncidentID == id);

            if (i == null) return NotFound();

            return Ok(new IncidentDto
            {
                IncidentID = i.IncidentID,
                AssignmentID = i.AssignmentID,
                Timestamp = i.Timestamp,
                Reason = i.Reason.ToString()
            });
        }

        // POST: api/incident
        [HttpPost]
        public async Task<ActionResult<IncidentDto>> CreateIncident([FromBody] IncidentCreateDto dto)
        {
            // guardrails
            var assignmentExists = await _context.Assignments
                .AsNoTracking()
                .AnyAsync(a => a.AssignmentID == dto.AssignmentID);

            if (!assignmentExists)
                return BadRequest($"Assignment {dto.AssignmentID} does not exist.");

            // default timestamp if not provided
            var timestamp = dto.Timestamp ?? DateTime.UtcNow;

            // parse enum (DTO sends reason as string for simplicity)
            if (!Enum.TryParse<IncidentReason>(dto.Reason, ignoreCase: true, out var reason))
                return BadRequest("Invalid incident reason.");

            var model = new Incident
            {
                AssignmentID = dto.AssignmentID,
                Timestamp = timestamp,
                Reason = reason
            };

            _context.Incidents.Add(model);
            await _context.SaveChangesAsync();

            var result = new IncidentDto
            {
                IncidentID = model.IncidentID,
                AssignmentID = model.AssignmentID,
                Timestamp = model.Timestamp,
                Reason = model.Reason.ToString()
            };

            // returns 201 with Location header: /api/incident/{id}
            return CreatedAtAction(nameof(GetIncident), new { id = model.IncidentID }, result);
        }

        // DELETE: api/incident/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var entity = await _context.Incidents.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Incidents.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}