using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync();

            // Veritabanından gelen Entity listesini DTO listesine çeviriyoruz (Mapping)
            var productDtos = products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Stock
            )).ToList();

            return productDtos;
        }
    }
}