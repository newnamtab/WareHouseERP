using Microsoft.EntityFrameworkCore;
using Persistence.ReadModels;

namespace Persistence.Repositories
{
    internal class StorageItemRepository : IStorageItemRepository
    {
        private readonly IApplicationDbContext _context;

        public StorageItemRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StorageItemRead>> GetStorageItems(StorageItemQuery query)
        {
            var filteredItems = _context.StorageItems
                .Include(si => si.StorageItemCategories)
                .Where( ItemsMatches(query) );
                
            return filteredItems.Select(item => new StorageItemRead(item.ProductId,
                                                    item.ItemId,
                                                    item.Sku,
                                                    item.Description,
                                                    item.Price));
        }
        private Func<Entities.StorageItem, bool> ItemsMatches(StorageItemQuery query)
        {
            return (item) => 
            {
                return query.PriceIntervalMatches(item.Price)
                    && query.MatchesCategories(item.StorageItemCategories.Select(x => x.Id));
                   // && query.MatchesStorageStatus( item. );
            };
        }
    }
}
