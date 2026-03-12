namespace WareHouseERP.Domain.Entities;

/// <summary>
/// Domain entity for Product with inventory management
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalStock { get; set; }
    public int ReservedStock { get; set; }

    public int AvailableStock => TotalStock - ReservedStock;

    private Product() { }

    public Product(string name, string sku, decimal price, int totalStock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required", nameof(sku));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than 0", nameof(price));
        if (totalStock < 0)
            throw new ArgumentException("Total stock cannot be negative", nameof(totalStock));

        Name = name;
        Sku = sku;
        Price = price;
        TotalStock = totalStock;
        ReservedStock = 0;
    }

    public bool ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        if (quantity > AvailableStock)
            return false;

        ReservedStock += quantity;
        return true;
    }

    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        ReservedStock = Math.Max(0, ReservedStock - quantity);
    }

    public void ConfirmReservation(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
        if (quantity > ReservedStock)
            throw new InvalidOperationException("Cannot confirm more than reserved stock");

        TotalStock -= quantity;
        ReservedStock -= quantity;
    }
}