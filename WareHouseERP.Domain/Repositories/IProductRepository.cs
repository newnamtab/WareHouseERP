namespace WareHouseERP.Domain.Repositories;

using Entities;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId);
    Task<Product?> GetBySkuAsync(string sku);
    Task<List<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int productId);
    Task<bool> ExistsAsync(int productId);
}