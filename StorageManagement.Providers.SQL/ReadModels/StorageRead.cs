using StorageManagement;

namespace StorageManagement.Providers.SQL.ReadModels
{
    public record StorageRead(Guid Id, string Description, int Capacity, IEnumerable<IStorageItem> StorageItems) : IStorage;
}                            