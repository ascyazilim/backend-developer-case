using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Products.Commands.CreateProduct;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Dependency Injection ile MediatR'ı alıyoruz
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            // İsteği alıyoruz ve Send metodu ile MediatR'a fırlatıyoruz.
            // MediatR otomatik olarak gidip CreateProductCommandHandler sınıfını bulacak ve çalıştıracak!
            var productId = await _mediator.Send(command);

            return Ok(new
            {
                Status = "Success",
                ProductId = productId,
                Message = "Ürün başarıyla eklendi."
            });
        }
    }
}