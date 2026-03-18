using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Log.API.Consumers;
using MassTransit;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog Konfigürasyonu (Serilog.Log kullanarak namespace çakışmasını önlüyoruz)
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();

// 1. MongoDB Kaydı (Dependency Injection)
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings");
var mongoClient = new MongoClient(mongoDbSettings["ConnectionString"]);
var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings["DatabaseName"]);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase); // Sisteme tekil (Singleton) olarak veritabanını tanıtıyoruz

// 2. MassTransit ve RabbitMQ Kaydı (Consumer ile birlikte)
builder.Services.AddMassTransit(x =>
{
    // Yazdığımız Consumer sınıfını MassTransit'e tanıtıyoruz
    x.AddConsumer<ProductCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // RabbitMQ üzerinde bu servis için bir "Kuyruk" (Queue) oluşturuyoruz ve dinlemeye başlıyoruz
        cfg.ReceiveEndpoint("log-product-created-queue", e =>
        {
            e.ConfigureConsumer<ProductCreatedEventConsumer>(context);
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

try
{
    // INFO seviyesi log
    Serilog.Log.Information("Log.API Servisi başlatılıyor...");
    app.Run();
}
catch (Exception ex)
{
    // CRITICAL seviyesi log
    Serilog.Log.Fatal(ex, "Uygulama beklenmedik bir şekilde çöktü!");
}
finally
{
    Serilog.Log.CloseAndFlush();
}