using Microsoft.AspNetCore.Mvc;

namespace Log.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                service = "Log.API",
                status = "Healthy",
                timestamp = DateTime.UtcNow 
            });
        }
    }
}
