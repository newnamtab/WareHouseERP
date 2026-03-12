namespace WareHouseERP.Domain.Repositories;

using Entities;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);
    Task<List<Order>> GetByUserIdAsync(string userId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int orderId);
}