using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Persistence.Context;

namespace Product.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        // Dependency Injection ile DbContext'i alıyoruz
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductEntity product)
        {
            // Veritabanına asenkron olarak ekleme yapıyoruz
            await _context.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            // Değişiklikleri asenkron olarak kaydediyoruz
            await _context.SaveChangesAsync();
        }
    }
}