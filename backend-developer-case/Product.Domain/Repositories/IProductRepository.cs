using Product.Domain.Entities;

namespace Product.Domain.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(ProductEntity product);
        Task SaveChangesAsync();
    }
}