namespace OrderManagement.Providers.SQL.Entities
{
    internal class Order
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
