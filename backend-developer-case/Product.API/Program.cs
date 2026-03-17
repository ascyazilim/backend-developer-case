using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Application.Features.Products.Commands.CreateProduct;
using Product.Domain.Repositories;
using Product.Infrastructure;
using Product.Infrastructure.Persistence.Context;
using Product.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);


// 1. Veritabanı Bağlantısı (DbContext) Ayarı
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductSqlConnection")));

// 2. Repository Kaydı (Dependency Injection)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 3. MediatR Kaydı
// CreateProductCommand sınıfının bulunduğu Application katmanını (Assembly) gösteriyoruz.
// MediatR bu katmandaki tüm Command, Query ve Handler'ları otomatik olarak bulup sisteme dahil edecek.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// ... (builder.Services.AddControllers(); satırı ve devamı)

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();