using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Contracts.Products;

namespace Product.Domain.Services
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(CreateProductRequest request);

        Task<ProductResponse?> GetByIdAsync(Guid id);

        Task<IEnumerable<ProductResponse>> GetAllAsync();
    }
}
