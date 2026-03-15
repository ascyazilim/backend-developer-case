using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Domain.Entities; 

namespace Product.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<ProductEntity> AddAsync(ProductEntity product);

        Task<ProductEntity?> GetByIdAsync(Guid id);

        Task<IEnumerable<ProductEntity>> GetAllAsync();
    }
}
