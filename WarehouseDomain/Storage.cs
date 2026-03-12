namespace StorageManagement
{
    internal class Storage
    {
        private readonly List<StorageItem> _storageItems;
        private readonly List<ReservedStorageItem> _reservedStorageItems;

        public Storage()
        {
            _storageItems = new List<StorageItem>();
            _reservedStorageItems = new List<ReservedStorageItem>();
        }
        public bool HasProductAvailable(Guid productId) => NoneReservedItems(productId)?.Any() ?? false;
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

        public StorageItem RemoveItem(Guid itemId)
        {
            if (TryGetReservedItem(itemId, out var reservedItem))
            {
                _reservedStorageItems.RemoveAll(reserved => reserved.ItemId == reservedItem.ItemId);
                _storageItems.RemoveAll(item => item.ItemId == reservedItem.ItemId);
                return reservedItem;
            }
            return StorageItem.Empty();
        }
        private bool TryGetReservedItem(Guid productId, out StorageItem storageItem)
        {
            var reservedStorageItem = _reservedStorageItems.FirstOrDefault(reserved => reserved.ProductId == productId);
            if (reservedStorageItem != null)
            {
                var item = _storageItems.FirstOrDefault(item => item.ItemId == reservedStorageItem.ItemId);
                if (item != null)
                {
                    storageItem = item;
                    return true;
                }
            }
            storageItem = StorageItem.Empty();
            return false;
        }

        public bool ReserveItem(Guid productId)
        {
            var storageItem = _storageItems.FirstOrDefault(item => item.ProductId == productId);
            if (storageItem != null)
            {
                _reservedStorageItems.Add(new ReservedStorageItem(storageItem.ProductId, storageItem.ItemId));
                return true;
            }
            return false;
        }
        private bool TryGetNoneReservedItem(Guid productId, out StorageItem storageItem)
        {
            var availableStorageItem = NoneReservedItems(productId)?.FirstOrDefault();

            if (availableStorageItem != null)
            {
                storageItem = availableStorageItem;
                return true;
            }
            storageItem = StorageItem.Empty();
            return false;
        }
        private IEnumerable<StorageItem> NoneReservedItems(Guid productId)
        {
            var reservedProducts = _reservedStorageItems.Where(reserved => reserved.ProductId == productId);
            return _storageItems.Where(item => item.ProductId == productId && !reservedProducts.Any(reserved => reserved.ItemId == item.ItemId));
        }
    }
}
