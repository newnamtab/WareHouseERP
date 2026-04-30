using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Providers.SQL
{
    internal interface IOrderDbContext
    {
        DbSet<Entities.Order> Orders{ get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    internal class OrderDbContext : DbContext, IOrderDbContext
    {
        public DbSet<Entities.Order> Orders { get; set; }
        
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Order>()
                        .OwnsMany(o => o.OrderLines);

            base.OnModelCreating(modelBuilder);
        }
    }
}
