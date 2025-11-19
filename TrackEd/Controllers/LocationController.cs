using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {

        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
        
        [HttpPost("SaveCoords")]
        public IActionResult SaveCoords([FromBody] GeoData coords)
        {

            if (coords == null)
                return BadRequest();

            _logger.LogInformation("{long}", coords.Longitude);
            _logger.LogInformation("{lat}", coords.Latitude);


            // SaveToSQLite(coords);
            return Ok();
        }

        // private void SaveToSQLite(GeoData data)
        // {
        //     using var conn = new SqliteConnection("Data Source=geo.db");
        //     conn.Open();

        //     var cmd = conn.CreateCommand();
        //     cmd.CommandText = 
        //         "INSERT INTO Locations (Latitude, Longitude, Timestamp) VALUES (@lat, @lng, CURRENT_TIMESTAMP)";
        //     cmd.Parameters.AddWithValue("@lat", data.Latitude);
        //     cmd.Parameters.AddWithValue("@lng", data.Longitude);

        //     cmd.ExecuteNonQuery();
        // }
    }

    public class GeoData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
