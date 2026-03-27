using Microsoft.EntityFrameworkCore;
using StorageManagement.Providers.SQL.Entities;
using StorageManagement.Providers.SQL.ReadModels;

namespace StorageManagement.Providers.SQL.Repositories
{
    internal class StorageProvider : IStorageProvider
    {
        private readonly IStorageDbContext _context;

        public StorageProvider(IStorageDbContext context)
        {
            _context = context;
        }

        public async Task<IStorage?> GetStorageById(Guid storageId)
        {
            var relevantStorage = await _context.Storages.FindAsync(storageId);
            return MapReadOut(relevantStorage);
        }

        public async Task<IStorage?> GetStorageWithProduct(Guid productId)
        {
            var relevantStorage = await _context.Storages.FirstOrDefaultAsync(s => s.StorageItems.Any(si => si.ProductId == productId));
            return MapReadOut(relevantStorage);
        }

        public async Task<IStorage?> GetStorageWithSpace()
        {
            var relevantStorage = await _context.Storages.FirstOrDefaultAsync(s => s.StorageItems.Count() < s.Capacity );
            return MapReadOut(relevantStorage);
        }
        private StorageRead? MapReadOut(Storage? entitystorage) => entitystorage != null
                                                                    ? new StorageRead(entitystorage.Id, entitystorage.Description, entitystorage.Capacity,
                                                                                      entitystorage.StorageItems.Select(si => new StorageItemRead(si.ProductId, si.ItemId, si.Sku, si.Description, si.Price))
                                                                                     )
                                                                    : null;
    }
}
