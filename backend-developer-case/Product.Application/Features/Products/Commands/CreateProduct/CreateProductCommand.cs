using MediatR;

namespace Product.Application.Features.Products.Commands.CreateProduct
{
    // IRequest<Guid>, bu komut işlendikten sonra geriye ürünün Id'sini (Guid) döneceğini belirtir.
    public record CreateProductCommand(string Name, decimal Price, int Stock) : IRequest<Guid>;
}