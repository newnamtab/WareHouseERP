namespace WareHouseERP.Application.DTOs;

public class CreateOrderRequest
{
    public string OrderNumber { get; set; } = string.Empty;
    public List<CreateOrderLineRequest> OrderLines { get; set; } = [];
}

public class CreateOrderLineRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}