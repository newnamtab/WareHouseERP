namespace Persistence.Entities;

internal class StorageItem
{
    public Guid ItemId { get; set; }
    public Guid ProductId { get; set; }
    public string Sku { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public ICollection<StorageItemCategory> StorageItemCategories { get; set; }

    public Guid StorageId { get; set; }
    public Storage Storage { get; set; }
}
