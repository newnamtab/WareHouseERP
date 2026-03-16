using Persistence.ReadModels;

namespace Persistence.Repositories
{
    internal class StorageItemRepository : IStorageItemRepository
    {
        public Task<IEnumerable<StorageItemRead>> GetStorageItems(object query)
        {
            throw new NotImplementedException();
        }
    }
}
