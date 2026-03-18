var builder = WebApplication.CreateBuilder(args);

// YARP servisini ekliyor ve ayarları appsettings.json içindeki "ReverseProxy" bölümünden okumasını söylüyoruz
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Gelen HTTP isteklerini YARP'a yönlendiriyoruz
app.MapReverseProxy();

app.Run();