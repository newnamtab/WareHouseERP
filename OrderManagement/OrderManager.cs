namespace OrderManagement;

public class OrderManager
{
    public Guid InitOrder()
    {
        return Guid.Empty;
    }
    public bool AddItemToOrder(Guid orderId, Guid productId)
    {
        return true;
    }
    public bool RemoveItemFromOrder(Guid orderId, Guid productId)
    {
        return true;
    }
    public bool FinalizeOrder(Guid orderId)
    {
        return true;
    }
}