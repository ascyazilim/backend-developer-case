using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Product.Domain.Repositories;

namespace Product.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache;

        public UpdateProductCommandHandler(IProductRepository productRepository, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Ürünü Guid ile veritabanından bul
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                return false; // Ürün yoksa false dön
            }

            // 2. Ürünün bilgilerini yeni gelen verilerle değiştir
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            // 3. EF Core değişikliği fark edecek, sadece kaydetmemiz yeterli!
            await _productRepository.SaveChangesAsync();

            await _cache.RemoveAsync("all_products_list", cancellationToken);

            return true;
        }
    }
}