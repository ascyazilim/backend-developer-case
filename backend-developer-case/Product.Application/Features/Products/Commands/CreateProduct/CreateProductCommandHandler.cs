using MassTransit;
using MediatR;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Shared.Contracts.Events; // Event sınıfımızın olduğu namespace

namespace Product.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint; // MassTransit arayüzü

        public CreateProductCommandHandler(IProductRepository repository, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
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

            return newProduct.Id;
        }
    }
}