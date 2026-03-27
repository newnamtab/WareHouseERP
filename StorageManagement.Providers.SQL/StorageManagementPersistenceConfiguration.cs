using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StorageManagement.Providers.SQL
{
    public static class StorageManagementPersistenceConfiguration
    {
        public static void ConfigureStorageManagement(this IServiceCollection services, string connectionString)
        {
           services.AddDbContext<StorageDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
