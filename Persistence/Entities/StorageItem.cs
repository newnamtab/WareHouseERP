namespace Persistence.Entities;

/// <summary>
/// Represents a product in the warehouse with inventory management
/// </summary>
internal class StorageItem
{
    public Guid ItemId { get; set; }
    public Guid ProductId { get; set; }
    public string Sku { get; set; }
    public string Description { get; set; }

    
    public Guid StorageId { get; set; }
    public Storage Storage { get; set; }
}
