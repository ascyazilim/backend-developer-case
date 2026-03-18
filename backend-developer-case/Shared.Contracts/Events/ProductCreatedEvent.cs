namespace Shared.Contracts.Events
{
    // Olay (Event) isimleri geçmiş zaman kipiyle yazılır (Eklendi, Silindi vb.)
    public record ProductCreatedEvent
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}