namespace OrderManagement.Providers.SQL.Entities
{
    internal class OrderLine
    {
        public Guid OrderLineId { get; set; }
        public Guid ProductId { get; set; }

        public Order Order { get; set; }
    }
}
