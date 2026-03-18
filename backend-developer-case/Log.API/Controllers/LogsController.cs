using Log.API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Log.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IMongoCollection<ProductLog> _logCollection;

        // Dependency Injection ile MongoDB'yi alıyoruz
        public LogsController(IMongoDatabase mongoDatabase)
        {
            _logCollection = mongoDatabase.GetCollection<ProductLog>("ProductLogs");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            // MongoDB'deki ProductLogs tablosundaki tüm kayıtları getiriyoruz
            var logs = await _logCollection.Find(_ => true).ToListAsync();

            return Ok(logs);
        }
    }
}