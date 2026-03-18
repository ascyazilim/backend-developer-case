using Log.API.Models;
using MassTransit;
using MongoDB.Driver;
using Shared.Contracts.Events;

namespace Log.API.Consumers
{
    // IConsumer arayüzüne hangi mesaj tipini dinleyeceğini söylüyoruz
    public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
    {
        private readonly IMongoCollection<ProductLog> _logCollection;

        // Dependency Injection ile MongoDB veritabanı nesnesini alıyoruz
        public ProductCreatedEventConsumer(IMongoDatabase mongoDatabase)
        {
            // appsettings'te belirttiğimiz "ProductLogs" koleksiyonuna (tablosuna) bağlanıyoruz
            _logCollection = mongoDatabase.GetCollection<ProductLog>("ProductLogs");
        }

        // RabbitMQ'dan mesaj geldiğinde MassTransit otomatik olarak bu metodu tetikler
        public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
        {
            var message = context.Message;

            // 1. Gelen Event'i MongoDB modelimize (ProductLog) dönüştürüyoruz
            var logEntry = new ProductLog
            {
                ProductId = message.ProductId,
                ProductName = message.ProductName,
                Price = message.Price,
                ProductCreatedDate = message.CreatedDate,
                LogReadeDate = DateTime.UtcNow // Logun işlendiği anki zaman
            };

            // 2. MongoDB'ye asenkron olarak kaydediyoruz
            await _logCollection.InsertOneAsync(logEntry);

            // 3. Konsola da bilgi yazdıralım ki çalıştığını API ekranında görelim
            Console.WriteLine($"[LOG SERVİSİ] Yeni ürün eklendi ve MongoDB'ye kaydedildi! Ürün Adı: {message.ProductName}");
        }
    }
}