using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Product.Application.Features.Products.Commands.CreateProduct;
using Product.Application.Features.Products.Queries.GetAllProducts;
using Product.Application.Features.Products.Queries.GetProductById;

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
        [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // Sadece boş bir Query nesnesi gönderiyoruz, MediatR gidip Handler'ı bulacak
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            // İsteği oluştur ve içine gelen Id'yi koy
            var query = new GetProductByIdQuery(id);

            // MediatR'a yolla
            var product = await _mediator.Send(query);

            // Eğer veritabanında o Id'ye ait ürün yoksa 404 dön
            if (product == null)
                return NotFound(new { Message = "Belirtilen Id'ye ait ürün bulunamadı." });

            // Varsa 200 OK ile ürünü dön
            return Ok(product);
        }
    }
}