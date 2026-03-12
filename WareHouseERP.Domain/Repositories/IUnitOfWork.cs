namespace WareHouseERP.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }

    Task<int> SaveChangesAsync();
    Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> operation);
}