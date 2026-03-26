namespace StorageManagement
{
    public interface IStorage
    {
        Guid Id { get; }
        string Description { get; }
        int Capacity { get; }
        IEnumerable<IStorageItem> StorageItems { get; }
    }
    public interface IStorageItem
    {
        Guid ProductId { get; }
        Guid ItemId { get; }
        string SKU { get; }
        string Description { get; }
        decimal Price { get; }
    }

    public interface IStorageRepository
    {
        //Task<IEnumerable<StorageRead>> GetStoragesByQuery(object query);
        Task<IStorage?> GetStorageWithSpace();
        Task<IStorage?> GetStorageWithProduct(Guid productId);
        Task<IStorage?> GetStorageById(Guid storageId);
    }

    public interface IProductStorageReservationRepository
    {
        Task<bool> RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId);
        Task<Guid> RetrieveStorageReservation(string forExternalReference, Guid productId);
        Task RemoveStorageReservation(string forExternalReference, Guid productId);
    }

    public interface IStorageItemWriteRepository
    {
        Task<bool> AddItemToStorage(IAddItemInformation itemInformation, Guid storageId);
        Task<Guid> PickItemFromStorage(Guid productId, Guid fromStorageId);
    }

    public interface IAddItemInformation
    {
        Guid ItemProductId { get; }
        string Sku { get; }
        string Description { get; }
        decimal Price { get; }
    }
}
