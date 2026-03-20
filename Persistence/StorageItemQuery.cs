namespace Persistence
{
    public class StorageItemQuery
    {
        private decimal? _minPrice;
        private decimal? _maxPrice;
        private IEnumerable<Guid>? _includesCategoryIds;
        private StorageItemQuery()
        {
        }
        public static StorageItemQuery Where()
        {
            return new StorageItemQuery();
        }
        public StorageItemQuery MinimumPriceIs(decimal minPrice)
        {
            this._minPrice = minPrice;
            return this;
        }
        public StorageItemQuery MaximumPriceIs(decimal maxPrice)
        {
            this._maxPrice = maxPrice;
            return this;
        }
        public StorageItemQuery CategoriesIncludes(params Guid[] categoryIds)
        {
            this._includesCategoryIds = categoryIds;
            return this;
        }

        internal bool PriceIntervalMatches(decimal price) =>
                 _minPrice.HasValue ? price >= _minPrice.Value
                                    : true
              && _maxPrice.HasValue ? price <= _maxPrice.Value
                                    : true;

        internal bool MatchesCategories(IEnumerable<Guid> categoryIds)
        { 
            if (categoryIds == null || !categoryIds.Any()) return true;
            
            return _includesCategoryIds!.Any(ici => categoryIds.Any(ci => ci == ici));
        }
    }
}
