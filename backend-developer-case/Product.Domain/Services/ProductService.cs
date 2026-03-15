using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Shared.Contracts.Products;

namespace Product.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var product = new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        };

        var created = await _repository.AddAsync(product);

        return new ProductResponse
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            Price = created.Price,
            Stock = created.Stock
        };
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        });
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}