
namespace StorageManagement
{
    internal record AddItemInformation(Guid ItemProductId, string Sku, string Description, decimal Price) : IAddItemInformation;
}
