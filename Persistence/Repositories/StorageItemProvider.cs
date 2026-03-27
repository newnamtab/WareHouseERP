using Microsoft.EntityFrameworkCore;
using StorageManagement;

namespace Persistence.Repositories
{
    internal class StorageItemProvider : IStorageItemProvider
    {

        private readonly IApplicationDbContext _context;

        public StorageItemProvider(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddItemToStorage(IAddItemInformation itemInformation, Guid storageId)
        {
            var relevantStorage = await _context.Storages
                                                .Include(s => s.StorageItems)
                                                .FirstOrDefaultAsync(s => s.Id == storageId);

            if (relevantStorage == null) { return false; }

            if (relevantStorage.StorageItems.Count() >= relevantStorage.Capacity) { return false; }

            var newStorageItem = new Entities.StorageItem
            {
                ItemId = Guid.NewGuid(),
                ProductId = itemInformation.ItemProductId,
                StorageId = storageId,
                Sku = itemInformation.Sku,
                Description = itemInformation.Description,
                Price = itemInformation.Price,
            };
            _context.StorageItems.Add(newStorageItem);
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<Guid> PickItemFromStorage(Guid productId, Guid fromStorageId)
        {
            var relevantStorage = await _context.Storages
                                                .Include(s => s.StorageItems)
                                                .FirstOrDefaultAsync(s => s.Id == fromStorageId);

            if (relevantStorage == null) { return Guid.Empty; }

            var relevantStorageItem = relevantStorage.StorageItems.FirstOrDefault(i => i.ProductId == productId);


            if (relevantStorageItem == null) { return Guid.Empty; }

            _context.StorageItems.Remove(relevantStorageItem);
            await _context.SaveChangesAsync();

            return relevantStorageItem.ItemId;
        }
    }
}
