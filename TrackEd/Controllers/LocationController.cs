using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        
        [HttpPost("SaveCoords")]
        public IActionResult SaveCoords([FromBody] GeoData coords)
        {
            if (coords == null)
                return BadRequest();

            Console.WriteLine(coords.Longitude);
            Console.WriteLine(coords.Longitude);


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
