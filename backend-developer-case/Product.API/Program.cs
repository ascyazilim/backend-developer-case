using MassTransit;
using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Application.Features.Products.Commands.CreateProduct;
using Product.Domain.Repositories;
using Product.Infrastructure;
using Product.Infrastructure.Persistence.Context;
using Product.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. SERILOG YAPILANDIRMASI (Structured JSON Logging & Centralized Seq)
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter()) // Konsola JSON yazdır
        .WriteTo.Seq("http://localhost:5341");// Logları merkezi Seq sunucusuna fırlat!
});

// 1. Veritabanı Bağlantısı (DbContext) Ayarı
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDb")));

// 2. Repository Kaydı (Dependency Injection)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 3. MediatR Kaydı
// CreateProductCommand sınıfının bulunduğu Application katmanını (Assembly) gösteriyoruz.
// MediatR bu katmandaki tüm Command, Query ve Handler'ları otomatik olarak bulup sisteme dahil edecek.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));



// RabbitMQ ve MassTransit Kaydı
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis Caching Entegrasyonu
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ProductCatalog_"; // Redis'te anahtarların başına bu ismi ekler (karışıklığı önler)
});

// 1. JWT Doğrulama Ayarları
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});
builder.Services.AddAuthorization();

// 2. Swagger'a Kilit Butonu Ekleme
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token'ı buraya yapıştırın."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

app.UseSerilogRequestLogging(); // Gelen istekleri (GET, POST vs.) profesyonelce loglar

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Kimlik doğrulama

app.UseAuthorization();  // Yetki kontrolü

app.UseHttpsRedirection();


app.MapControllers();

// --- DOCKER İÇİN OTOMATİK VERİTABANI OLUŞTURMA ---
using (var scope = app.Services.CreateScope())
{
    // Kendi Context adın neyse (örn: ProductDbContext veya ApplicationDbContext) onu yaz
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    dbContext.Database.Migrate();
}

app.Run();