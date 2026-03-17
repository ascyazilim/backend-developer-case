using MediatR;

namespace Product.Application.Features.Products.Queries.GetAllProducts
{
    // Dışarıya döneceğimiz veri modeli (DTO)
    public record ProductDto(Guid Id, string Name, decimal Price, int Stock);

    // List<ProductDto> döneceğini belirten Query nesnemiz
    public record GetAllProductsQuery() : IRequest<List<ProductDto>>;
}