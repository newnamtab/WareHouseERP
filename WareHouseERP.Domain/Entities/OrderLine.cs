namespace WareHouseERP.Domain.Entities;

/// <summary>
/// Domain entity for OrderLine
/// </summary>
public class OrderLine
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }

    private OrderLine() { }

    public OrderLine(int productId, string productName, string productSku, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));
        if (string.IsNullOrWhiteSpace(productSku))
            throw new ArgumentException("Product SKU is required", nameof(productSku));
        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than 0", nameof(unitPrice));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

        ProductId = productId;
        ProductName = productName;
        ProductSku = productSku;
        UnitPrice = unitPrice;
        Quantity = quantity;
        LineTotal = unitPrice * quantity;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(newQuantity));

        Quantity = newQuantity;
        LineTotal = UnitPrice * Quantity;
    }
}