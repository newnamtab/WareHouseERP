namespace Persistence.Entities
{
    internal class ProductStorageReservation
    {
        public string ExternalReference { get; set; }
        public Guid ProductId { get; set; }
        public Guid StorageId { get; set; }
        public DateTime ReservedAt { get; set; }
    }
}
