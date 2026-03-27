namespace StorageManagement.Providers.SQL.Entities
{
    internal class ItemStorageReservation
    {
        public string ExternalReference { get; set; }
        public Guid ProductId { get; set; }
        public Guid StorageId { get; set; }
        public DateTime ReservedAt { get; set; }
    }
}
