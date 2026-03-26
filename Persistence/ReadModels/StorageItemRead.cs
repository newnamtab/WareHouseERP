using StorageManagement;

namespace Persistence.ReadModels
{
    public record StorageItemRead(Guid ProductId, Guid ItemId, string SKU, string Description, decimal Price) : IStorageItem;
}                               