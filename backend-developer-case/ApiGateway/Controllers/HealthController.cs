using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
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
                service = "ApiGateway",
                status = "Healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}