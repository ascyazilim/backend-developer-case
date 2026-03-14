using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
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
                service = "Auth.API",
                status = "Healthy",
                timestamp = DateTime.UtcNow  
            });
        }
    }
}
