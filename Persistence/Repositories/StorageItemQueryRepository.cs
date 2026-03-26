using Microsoft.EntityFrameworkCore;
using Persistence.ReadModels;

namespace Persistence.Repositories
{
    public interface IStorageItemQueryRepository
    {
        Task<IEnumerable<StorageItemRead>> QueryStorageItems(StorageItemQuery query);
    }

    internal class StorageItemQueryRepository : IStorageItemQueryRepository
    {
        private readonly IApplicationDbContext _context;

        public StorageItemQueryRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StorageItemRead>> QueryStorageItems(StorageItemQuery query)
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
