using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Product.Domain.Repositories;
using System.Text.Json;
using Product.Application.DTOs;


namespace Product.Application.Features.Products.Queries.GetAllProducts
{
    // Handler'ımız IRequest<List<ProductDto>> bekleyen Query'mize cevap veriyor
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "all_products_list";

        public GetAllProductsQueryHandler(IProductRepository productRepository, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. Önce Redis'e (Önbelleğe) bak
            var cachedProductsString = await _cache.GetStringAsync(CacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedProductsString))
            {
                // Redis'te varsa, JSON'ı List<ProductDto> olarak çöz ve hemen dön!
                return JsonSerializer.Deserialize<List<ProductDto>>(cachedProductsString)!;
            }

            // 2. Redis'te YOKSA, veritabanından Entity olarak çek
            var productsFromDb = await _productRepository.GetAllAsync();

            // 3. Entity listesini DTO listesine çevir (Mapping)
            var productDtos = productsFromDb.Select(p =>
                new ProductDto(p.Id, p.Name, p.Price, p.Stock)
            ).ToList();

            // 4. DTO listesini Redis'e yaz (Bir dahaki sefere hızlı gelsin diye)
            var serializedProducts = JsonSerializer.Serialize(productDtos);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

            await _cache.SetStringAsync(CacheKey, serializedProducts, cacheOptions, cancellationToken);

            // 5. DTO'yu kullanıcıya dön
            return productDtos;
        }
    }
}