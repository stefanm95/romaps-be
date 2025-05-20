using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using romaps_be.Models;
using romaps_be.Settings;

namespace romaps_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly IMongoCollection<Camera> _cameras;

        public CameraController(IMongoClient client, IOptions<MongoDbSettings> settings)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _cameras = database.GetCollection<Camera>("Cameras");
        }

        [HttpGet]
        public async Task<IActionResult> GetCameras()
        {
            var cameras = await _cameras.Find(_ => true).ToListAsync();
            return Ok(cameras);
        }
    }
}
