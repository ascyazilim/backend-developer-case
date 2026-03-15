using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Domain.Services;
using Shared.Contracts.Products;

namespace Product.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            var createdProduct = await _productRepository.AddAsync(product);

            return new ProductResponse
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock
            };
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(product => new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            });
        }

        public async Task<ProductResponse?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return null;

            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}