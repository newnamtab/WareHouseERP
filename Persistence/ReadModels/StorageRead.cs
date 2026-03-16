namespace Persistence.ReadModels
{
    public record StorageRead(Guid Id, string Description, int Capacity, IEnumerable<StorageItemRead> StorageItems);
}                            