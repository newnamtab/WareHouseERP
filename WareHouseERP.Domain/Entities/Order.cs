namespace WareHouseERP.Domain.Entities;

/// <summary>
/// Domain entity for Order
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<OrderLine> OrderLines { get; private set; } = [];
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    private Order() { }

    public Order(string userId, string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId is required", nameof(userId));
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("OrderNumber is required", nameof(orderNumber));

        UserId = userId;
        OrderNumber = orderNumber;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddOrderLine(OrderLine orderLine)
    {
        if (orderLine == null)
            throw new ArgumentNullException(nameof(orderLine));

        OrderLines.Add(orderLine);
        RecalculateTotal();
    }

    public void RemoveOrderLine(int orderLineId)
    {
        var line = OrderLines.FirstOrDefault(l => l.Id == orderLineId);
        if (line != null)
        {
            OrderLines.Remove(line);
            RecalculateTotal();
        }
    }

    public void ConfirmOrder()
    {
        if (!OrderLines.Any())
            throw new InvalidOperationException("Cannot confirm order without order lines");

        Status = OrderStatus.Confirmed;
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Cancelled || Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
            throw new InvalidOperationException($"Cannot cancel order with status {Status}");

        Status = OrderStatus.Cancelled;
    }

    public void ShipOrder()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be shipped");

        Status = OrderStatus.Shipped;
    }

    public void DeliverOrder()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be delivered");

        Status = OrderStatus.Delivered;
    }

    private void RecalculateTotal()
    {
        TotalAmount = OrderLines.Sum(line => line.LineTotal);
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}