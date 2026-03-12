namespace StorageManagement
{
    internal record StorageItem
    {
        public Guid ItemId { get; init; }
        public Guid ProductId { get; init; }

        private StorageItem(Guid itemId, Guid productId)
        {
            ItemId = itemId;
            ProductId = productId;
        }
        public static StorageItem NewItem(Guid productId) => new StorageItem(productId, Guid.NewGuid() );
        public static StorageItem Empty() => new StorageItem(Guid.Empty, Guid.Empty);
    }
    internal record ReservedStorageItem(Guid ProductId, Guid ItemId);
  
}
