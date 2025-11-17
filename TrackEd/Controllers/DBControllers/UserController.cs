using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;
using TrackEd.Models.DTOs;

namespace TrackEd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        // Returns ALL app users (Students, Professors, and any base User rows)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            var users = await _context.AppUsers
                .AsNoTracking()
                .Select(u => new UserReadDto
                {
                    Enumber     = u.Enumber,
                    EtsuEmail   = u.EtsuEmail,
                    FirstName   = u.FirstName,
                    LastName    = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    // Read the TPH discriminator column configured in OnModelCreating
                    UserType    = EF.Property<string>(u, "UserType")
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/user/123456
        [HttpGet("{enumber:int}")]
        public async Task<ActionResult<UserReadDto>> GetByEnumber(int enumber)
        {
            var u = await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Enumber == enumber);

            if (u == null)
                return NotFound($"User {enumber} not found.");

            var dto = new UserReadDto
            {
                Enumber     = u.Enumber,
                EtsuEmail   = u.EtsuEmail,
                FirstName   = u.FirstName,
                LastName    = u.LastName,
                PhoneNumber = u.PhoneNumber,
                UserType    = EF.Property<string>(u, "UserType")
            };

            return Ok(dto);
        }

        // PUT: api/user/123456
        // Generic update for shared User fields, regardless of subtype (Student/Professor/User)
        [HttpPut("{enumber:int}")]
        public async Task<IActionResult> Update(int enumber, [FromBody] UserUpdateDto dto)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Enumber == enumber);

            if (user == null)
                return NotFound($"User {enumber} not found.");

            // Optional email update with uniqueness guard
            if (!string.IsNullOrWhiteSpace(dto.EtsuEmail) &&
                dto.EtsuEmail != user.EtsuEmail)
            {
                var emailInUse = await _context.AppUsers
                    .AnyAsync(u => u.EtsuEmail == dto.EtsuEmail && u.Enumber != enumber);

                if (emailInUse)
                    return BadRequest($"Email {dto.EtsuEmail} is already in use.");

                user.EtsuEmail = dto.EtsuEmail;
            }

            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            // For this project only – no hashing, just store what they give you.
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = dto.Password;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // NOTE:
        // - No POST here because User is abstract; create Students via StudentController
        //   and Professors via ProfessorController.
        // - If you REALLY wanted a generic create, you'd need a non-abstract concrete type.
    }
}