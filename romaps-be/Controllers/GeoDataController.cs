using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;


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

            var files = Directory.GetFiles(highwaysDir, "*.geojson");
            var highways = files.Select(file => System.IO.File.ReadAllText(file)).ToList();

            return Content($"[{string.Join(",", highways)}]", "application/json");
        }

        [HttpGet("borders")]
        public IActionResult GetBordersGeoJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Roboundary", "ro-border.geojson");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var geoJson = System.IO.File.ReadAllText(filePath);
            return Content(geoJson, "application/json");
        }

        [HttpGet("nationalRoads")]
        public IActionResult GetNationalRoadsGeoJson()
        {
            var roadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "NationalRoads");
            if (!Directory.Exists(roadsDir))
                return NotFound();

            var files = Directory.GetFiles(roadsDir, "*.geojson");
            var roads = files.Select(file => System.IO.File.ReadAllText(file)).ToList();

            return Content($"[{string.Join(",", roads)}]", "application/json");
        }

        [HttpGet("nationalRoads/filter")]
        public IActionResult GetFilteredNationalRoads([FromQuery] string refValue)
        {
            if (string.IsNullOrEmpty(refValue))
                return BadRequest("Query parameter 'refValue' is required.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "Data", "NationalRoads", "merged_national_roads.geojson");
            if (!System.IO.File.Exists(filePath))
                return NotFound("GeoJson file not found.");

            var geoJsonText = System.IO.File.ReadAllText(filePath);
            var root = JObject.Parse(geoJsonText);

            var features = root["features"] as JArray;
            if (features == null)
                return Ok(new { type = "FeatureCollection", features = new JArray() });

            var filteredFeatures = new JArray();

            foreach (var feature in features)
            {
                var properties = feature["properties"];
                if (properties == null || properties["ref"] == null)
                    continue;

                var refTag = properties["ref"].ToString();       // e.g. "DN1;DN7"
                var parts = refTag.Split(';');                    // ["DN1","DN7"]

                foreach (var part in parts)
                {
                    if (part.Equals(refValue, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredFeatures.Add(feature);
                        break;
                    }
                }
            }

            var filteredGeoJson = new JObject
            {
                ["type"] = "FeatureCollection",
                ["features"] = filteredFeatures
            };

            return Content(filteredGeoJson.ToString(), "application/json");
        }
    }
}
