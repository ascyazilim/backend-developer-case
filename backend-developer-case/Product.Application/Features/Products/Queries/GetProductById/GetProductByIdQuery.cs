using MediatR;

namespace Product.Application.Features.Products.Queries.GetProductById
{
    // Detay sayfasında belki CreatedDate'i de göstermek isteriz diye farklı bir DTO tanımlıyoruz
    public record ProductDetailDto(Guid Id, string Name, decimal Price, int Stock, DateTime CreatedDate);

    // Dışarıdan bir 'Id' (Guid) bekleyen ve geriye ProductDetailDto dönecek olan Query
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailDto?>;
}