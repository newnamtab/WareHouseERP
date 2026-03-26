
namespace StorageManagement
{
    public record AddItemInformation(Guid ItemProductId, string Sku, string Description, decimal Price) : IAddItemInformation;
}
