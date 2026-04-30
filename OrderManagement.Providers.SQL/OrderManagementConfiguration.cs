using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Providers.SQL
{
    public static class OrderManagementConfiguration
    {
        public static void ConfigureOrderManagement(this IServiceCollection services, string connectionString)
        {
           services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
