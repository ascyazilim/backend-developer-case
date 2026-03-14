using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
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
                service = "Product.API",
                status = "Healthy",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
