namespace StorageManagement
{
    public interface IStorage
    {
        Guid Id { get; }

        bool HasProductAvailable(Guid productId);
        bool HasStorageSpace();
        bool AddItem(Guid productId);
        StorageItem PickItem(Guid productId);
    }

    internal class StorageManager : IStorageManager
    {
        private readonly IEnumerable<IStorage> _storages;

        public StorageManager(IEnumerable<IStorage> storages)
        {
            _storages = storages;
        }

        public IStorage? GetStorageById(Guid storageId)
        {
            return _storages.FirstOrDefault(storage => storage.Id == storageId);
        }
        public IStorage? GetStorageWithSpace() 
        {
            return _storages.FirstOrDefault(storage => storage.HasStorageSpace());
        }
        public IStorage? GetStorageWithProduct(Guid productId)
        {
            return _storages.FirstOrDefault(storage => storage.HasProductAvailable(productId));
        }
    }
}
