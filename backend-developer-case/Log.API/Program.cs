using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

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