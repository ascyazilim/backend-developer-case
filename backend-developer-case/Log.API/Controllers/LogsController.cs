using Microsoft.AspNetCore.Mvc;

namespace Log.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateLog([FromBody] LogRequest request)
        {
            // Gelen isteğin seviyesine göre logluyoruz
            switch (request.Level.ToUpper())
            {
                case "INFO":
                    _logger.LogInformation("Gelen Log: {Message} | Kaynak: {Source}", request.Message, request.Source);
                    break;
                case "WARNING":
                    _logger.LogWarning("Gelen Uyarı: {Message} | Kaynak: {Source}", request.Message, request.Source);
                    break;
                case "ERROR":
                    _logger.LogError("Gelen Hata: {Message} | Kaynak: {Source}", request.Message, request.Source);
                    break;
                case "CRITICAL":
                    _logger.LogCritical("Gelen Kritik Hata: {Message} | Kaynak: {Source}", request.Message, request.Source);
                    break;
                default:
                    _logger.LogInformation("Gelen Bilinmeyen Log: {Message} | Kaynak: {Source}", request.Message, request.Source);
                    break;
            }

            return Ok(new { Result = "Log başarıyla kaydedildi." });
        }
    }

    // Gelen JSON verisini karşılayacak DTO sınıfımız
    public class LogRequest
    {
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}