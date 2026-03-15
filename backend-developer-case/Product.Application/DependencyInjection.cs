using Microsoft.Extensions.DependencyInjection;
using Product.Application.Services;
using Product.Domain.Services;

namespace Product.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProductApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}