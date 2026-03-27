using Microsoft.EntityFrameworkCore;
using StorageManagement.Providers.SQL.Entities;
using StorageManagement.Providers.SQL.ReadModels;
using StorageManagement;

namespace StorageManagement.Providers.SQL.Repositories
{
    internal class StorageItemQueryProvider : IStorageItemQueryProvider
    {
        private readonly IStorageDbContext _context;

        public StorageItemQueryProvider(IStorageDbContext context)
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
