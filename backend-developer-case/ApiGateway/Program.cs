using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//HIZ SINIRI (IP TABANLI)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // "KatiKural" adında, kullanıcının IP adresine göre çalışan bir politika (Policy) yazıyoruz
    options.AddPolicy("KatiKural", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // İstek atan kişinin IP adresini alıyoruz. Bulamazsa "unknown" diyoruz.
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20, // 10 saniyede sadece 20 istek hakkı
                Window = TimeSpan.FromSeconds(10),
                QueueLimit = 0 // Sıraya alma, anında 429 hatası fırlat
            }));
});

// YARP Ayarları
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();
app.UseRateLimiter(); // Polis IP'yi kontrol edecek
app.MapReverseProxy();



app.Run();