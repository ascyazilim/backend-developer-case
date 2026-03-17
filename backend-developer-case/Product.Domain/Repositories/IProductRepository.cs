using Product.Domain.Entities;

namespace Product.Domain.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(ProductEntity product);
        Task SaveChangesAsync();

        Task<IEnumerable<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(Guid id);
    }
}