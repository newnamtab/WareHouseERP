using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class StorageManagementPersistenceConfiguration
    {
        public static void ConfigureStorageManagement(this IServiceCollection services, string connectionString)
        {
           services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
