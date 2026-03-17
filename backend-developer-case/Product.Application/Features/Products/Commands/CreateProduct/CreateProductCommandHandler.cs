using MediatR;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        // Dependency Injection ile Repository'i alıyoruz
        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        // MediatR bu metodu asenkron olarak tetikleyecek
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Yeni ürün objesini oluştur
            var newProduct = new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                CreatedDate = DateTime.UtcNow
            };

            // 2. Veritabanına asenkron olarak ekle
            await _repository.AddAsync(newProduct);
            await _repository.SaveChangesAsync();

          

            return newProduct.Id;
        }
    }
}