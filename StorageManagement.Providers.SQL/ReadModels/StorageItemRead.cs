using StorageManagement;

namespace StorageManagement.Providers.SQL.ReadModels
{
    public record StorageItemRead(Guid ProductId, Guid ItemId, string SKU, string Description, decimal Price) : IStorageItem;
}                               