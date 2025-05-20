using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace romaps_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoDataController : ControllerBase
    {
        [HttpGet("highways")]
        public IActionResult GetHighways()
        {
            var highwaysDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Highways");
            if (!Directory.Exists(highwaysDir))
                return NotFound();

            var files = Directory.GetFiles(highwaysDir, "*.json");
            var highways = files.Select(file => System.IO.File.ReadAllText(file)).ToList();

            // If each file is a Feature or FeatureCollection, you may want to combine them into a single array or FeatureCollection.
            return Content($"[{string.Join(",", highways)}]", "application/json");
        }

        [HttpGet("borders")]
        public IActionResult GetBordersGeoJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Roboundary", "romania_Romania_Country_Boundary.geojson");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var geoJson = System.IO.File.ReadAllText(filePath);
            return Content(geoJson, "application/json");
        }
    }
}
