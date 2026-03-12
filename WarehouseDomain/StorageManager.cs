namespace StorageManagement
{
    internal class StorageManager
    {
        private readonly IEnumerable<Storage> _storages;

        private StorageManager(IEnumerable<Storage> storages)
        {
            _storages = storages;
        }
        public bool ReserveProduct(Guid productId)
        {
            var relevantStorage = _storages.FirstOrDefault(storage => storage.HasProductAvailable(productId));
            if (relevantStorage != null)
            {
                return relevantStorage.ReserveItem(productId);
            }
            return false;
        }

        public bool ProductOut(Guid productId)
        {
            throw new NotImplementedException();
        }

        public bool ProductIn(Guid productId)
        {
            var relevantStorage = _storages.FirstOrDefault(storage => storage.HasStorageSpace(productId));
            if (relevantStorage != null)
            {
                return relevantStorage.AddItem(productId);
            }
            return false;
        }
    }
}
