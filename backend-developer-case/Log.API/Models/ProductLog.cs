using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Log.API.Models
{
    public class ProductLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // MongoDB'nin kendi otomatik oluşturacağı eşsiz ID

        [BsonRepresentation(BsonType.String)]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime ProductCreatedDate { get; set; }

        public DateTime LogReadeDate { get; set; } = DateTime.UtcNow; // Logun okunduğu/kaydedildiği an
    }
}