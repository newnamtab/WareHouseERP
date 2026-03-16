namespace StorageManagement
{
    internal class Storage : IStorage
    {
        private readonly int _capacity;
        private readonly List<StorageItem> _storageItems;

        public Guid Id { get; private set; }

        public Storage(int capacity)
        {
            _storageItems = new List<StorageItem>();

            Id = capacity > 0 ? Guid.NewGuid() :Guid.Empty;
            _capacity = capacity;
        }
        public bool HasProductAvailable(Guid productId) => TryGetItem(productId, out var storageItem);
        public bool HasStorageSpace() => _storageItems.Count() < _capacity;
        public bool AddItem(Guid productId)
        {
            if (HasStorageSpace())
            {
                _storageItems.Add(StorageItem.NewItem(productId));
                return true;
            }
            return false;
        }

        public StorageItem PickItem(Guid productId)
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
            var availableStorageItem = _storageItems.FirstOrDefault( i => i.ProductId == productId);

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
