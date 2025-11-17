using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackEd.Data;
using TrackEd.Models.Entities;

namespace TrackEd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            if (locations == null || !locations.Any())
            {
                return NotFound("No locations found.");
            }

            return Ok(locations);
        }

        // GET: api/location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound($"Location with ID {id} not found.");
            }

            return Ok(location);
        }
    }
}
// Example front end call 
// const response = await fetch("/api/location/3");
// const location = await response.json();
// console.log(location.latitude, location.longitude);

// Example C# call to compare location to GPS data 
// var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationID == assignment.LocationID);
// if (location != null)
// {
//    double lat = location.Latitude;
//    double lon = location.Longitude;
//   double radius = location.RadiusMeters;
//    // do your distance math here
// }