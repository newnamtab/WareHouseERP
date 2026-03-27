using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.ReadModels;
using StorageManagement;

namespace Persistence.Repositories
{
    internal class StorageItemQueryProvider : IStorageItemQueryProvider
    {
        private readonly IApplicationDbContext _context;

        public StorageItemQueryProvider(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IStorageItem>> QueryStorageItems(IStorageItemQuery query)
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
        private Func<StorageItem, bool> ItemsMatches(IStorageItemQuery query)
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
