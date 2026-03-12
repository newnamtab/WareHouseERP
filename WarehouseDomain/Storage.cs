namespace StorageManagement
{
    internal class Storage
    {

        private readonly List<StorageItem> _storageItems;

        public Guid Id { get; private set; }

        public Storage()
        {
            _storageItems = new List<StorageItem>();

            Id = Guid.NewGuid();
        }
        public bool HasProductAvailable(Guid productId) => TryGetItem(productId, out var storageItem);
        public bool HasStorageSpace(Guid productId) => _storageItems.Count() < 100; // Assuming a max capacity of 100 items per product
        public bool AddItem(Guid productId)
        {
            if (HasStorageSpace(productId))
            {
                _storageItems.Add(StorageItem.NewItem(productId));
                return true;
            }
            return false;
        }

        public StorageItem RemoveItem(Guid productId)
        {
            if (TryGetItem(productId, out var storageItem))
            {
                _storageItems.RemoveAll(item => item.ItemId == storageItem.ItemId);
                return storageItem;
            }
            return StorageItem.Empty();
        }
        private bool TryGetItem(Guid productId, out StorageItem storageItem)
        {
            var availableStorageItem = _storageItems.FirstOrDefault( i=>i.ProductId == productId);

            if (availableStorageItem != null)
            {
                storageItem = availableStorageItem;
                return true;
            }
            storageItem = StorageItem.Empty();
            return false;
        }
    }
}
