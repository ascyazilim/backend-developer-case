using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Persistence.Context;

namespace Product.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> AddAsync(ProductEntity product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}