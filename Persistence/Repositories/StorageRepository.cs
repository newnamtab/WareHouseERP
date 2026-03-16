using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.ReadModels;

namespace Persistence.Repositories
{
    internal class StorageRepository : IStorageRepository
    {
        private readonly ApplicationDbContext _context;

        public StorageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StorageRead?> GetStorageById(Guid storageId)
        {
            var relevantStorage = await _context.Storages.FindAsync(storageId);
            return MapReadOut(relevantStorage);
        }

        public async Task<StorageRead?> GetStorageWithProduct(Guid productId)
        {
            var relevantStorage = await _context.Storages.FirstOrDefaultAsync(s => s.StorageItems.Any(si => si.ProductId == productId));
            return MapReadOut(relevantStorage);
        }

        public async Task<StorageRead?> GetStorageWithSpace()
        {
            var relevantStorage = await _context.Storages.FirstOrDefaultAsync(s => s.StorageItems.Count() < s.Capacity );
            return MapReadOut(relevantStorage);
        }
        private StorageRead? MapReadOut(Storage? entitystorage) => entitystorage != null
                                                                    ? new StorageRead(entitystorage.Id, entitystorage.Description, entitystorage.Capacity,
                                                                                      entitystorage.StorageItems.Select(si => new StorageItemRead(si.ProductId, si.ItemId, si.Sku, si.Description))
                                                                                     )
                                                                    : null;
    }
}
