using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailDto?>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDetailDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Veritabanından ürünü Id'ye göre çek
            var product = await _repository.GetByIdAsync(request.Id);

            // 2. Eğer ürün yoksa null dön (Controller'da 404 NotFound vereceğiz)
            if (product == null) return null;

            // 3. Entity'i DTO'ya çevir (Mapleme)
            return new ProductDetailDto(
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate
            );
        }
    }
}