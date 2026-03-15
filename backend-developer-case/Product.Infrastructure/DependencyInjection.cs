using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.Repositories;
using Product.Infrastructure.Persistence.Context;
using Product.Infrastructure.Persistence.Repositories;

namespace Product.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProductInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProductDb")));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}