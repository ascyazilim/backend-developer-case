using MassTransit;
using MediatR;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Shared.Contracts.Events; // Event sınıfımızın olduğu namespace
using Microsoft.Extensions.Caching.Distributed;

namespace Product.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint; // MassTransit arayüzü
        private readonly IDistributedCache _cache;

        public CreateProductCommandHandler(IProductRepository repository, IPublishEndpoint publishEndpoint, IDistributedCache cache)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _cache = cache;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(newProduct);
            await _repository.SaveChangesAsync();

            // Ürün veritabanına eklendi, şimdi olayı (Event) RabbitMQ'ya fırlatıyoruz!
            await _publishEndpoint.Publish(new ProductCreatedEvent
            {
                ProductId = newProduct.Id,
                ProductName = newProduct.Name,
                Price = newProduct.Price,
                CreatedDate = newProduct.CreatedDate
            }, cancellationToken);

            await _cache.RemoveAsync("all_products_list", cancellationToken);

            return newProduct.Id;
        }
    }
}