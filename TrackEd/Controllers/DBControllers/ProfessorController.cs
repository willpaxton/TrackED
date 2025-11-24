using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;
using TrackEd.Models.DTOs;

namespace TrackEd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/professor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfessorReadDto>>> GetAll()
        {
            var profs = await _context.AppUsers
                .OfType<Professor>()
                .AsNoTracking()
                .Select(p => new ProfessorReadDto
                {
                    Enumber     = p.Enumber,
                    EtsuEmail   = p.EtsuEmail,
                    FirstName   = p.FirstName,
                    LastName    = p.LastName,
                    PhoneNumber = p.PhoneNumber,
                    IsAdmin     = p.IsAdmin
                })
                .ToListAsync();

            return Ok(profs);
        }

        // GET: api/professor/12345
        [HttpGet("{enumber:int}")]
        public async Task<ActionResult<ProfessorReadDto>> GetByEnumber(int enumber)
        {
            var p = await _context.AppUsers
                .OfType<Professor>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Enumber == enumber);

            if (p is null)
                return NotFound($"Professor {enumber} not found.");

            var dto = new ProfessorReadDto
            {
                Enumber     = p.Enumber,
                EtsuEmail   = p.EtsuEmail,
                FirstName   = p.FirstName,
                LastName    = p.LastName,
                PhoneNumber = p.PhoneNumber,
                IsAdmin     = p.IsAdmin
            };

            return Ok(dto);
        }

        // GET: api/professor/by-email?email=prof1@etsu.edu
        [HttpGet("by-email")]
        public async Task<ActionResult<ProfessorReadDto>> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var p = await _context.AppUsers
                .OfType<Professor>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EtsuEmail == email);

            if (p is null)
                return NotFound($"Professor with email {email} not found.");

            var dto = new ProfessorReadDto
            {
                Enumber     = p.Enumber,
                EtsuEmail   = p.EtsuEmail,
                FirstName   = p.FirstName,
                LastName    = p.LastName,
                PhoneNumber = p.PhoneNumber,
                IsAdmin     = p.IsAdmin
            };

            return Ok(dto);
        }

        // POST: api/professor
        [HttpPost]
        public async Task<ActionResult<ProfessorReadDto>> Create([FromBody] ProfessorCreateDto dto)
        {
            // Guard rails: unique Enumber + email
            var enumberExists = await _context.AppUsers
                .AnyAsync(u => u.Enumber == dto.Enumber);

            if (enumberExists)
                return BadRequest($"ENumber {dto.Enumber} is already in use.");

            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.EtsuEmail == dto.EtsuEmail);

            if (emailExists)
                return BadRequest($"Email {dto.EtsuEmail} is already in use.");

            var prof = new Professor
            {
                Enumber     = dto.Enumber,
                EtsuEmail   = dto.EtsuEmail,
                FirstName   = dto.FirstName,
                LastName    = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                IsAdmin     = dto.IsAdmin
                // PasswordHash will just use whatever default you set on User
            };

            _context.AppUsers.Add(prof); // TPH: Professors live in AppUsers table
            await _context.SaveChangesAsync();

            var readDto = new ProfessorReadDto
            {
                Enumber     = prof.Enumber,
                EtsuEmail   = prof.EtsuEmail,
                FirstName   = prof.FirstName,
                LastName    = prof.LastName,
                PhoneNumber = prof.PhoneNumber,
                IsAdmin     = prof.IsAdmin
            };

            return CreatedAtAction(
                nameof(GetByEnumber),
                new { enumber = prof.Enumber },
                readDto
            );
        }

        // PUT: api/professor/12345
        [HttpPut("{enumber:int}")]
        public async Task<ActionResult> Update(int enumber, [FromBody] ProfessorUpdateDto dto)
        {
            var prof = await _context.AppUsers
                .OfType<Professor>()
                .FirstOrDefaultAsync(p => p.Enumber == enumber);

            if (prof is null)
                return NotFound($"Professor {enumber} not found.");

            // Only update what the DTO actually exposes
            prof.PhoneNumber = dto.PhoneNumber;
            prof.IsAdmin     = dto.IsAdmin;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/professor/12345
        [HttpDelete("{enumber:int}")]
        public async Task<ActionResult> Delete(int enumber)
        {
            var prof = await _context.AppUsers
                .OfType<Professor>()
                .FirstOrDefaultAsync(p => p.Enumber == enumber);

            if (prof is null)
                return NotFound($"Professor {enumber} not found.");

            _context.AppUsers.Remove(prof);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}