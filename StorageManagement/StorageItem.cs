namespace StorageManagement
{
    public record StorageItem
    {
        public Guid ItemId { get; init; }
        public Guid ProductId { get; init; }

        public StorageItem( Guid productId, Guid itemId)
        {
            ItemId = itemId;
            ProductId = productId;
        }
        public static StorageItem Empty() => new StorageItem(Guid.Empty, Guid.Empty);
    }
}