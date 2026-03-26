namespace Persistence.Repositories
{
    public interface IAddItemInformation
    {
        Guid ItemProductId { get; }
        string Sku { get; }
        string Description { get; }
        decimal Price { get; }
    }
}
