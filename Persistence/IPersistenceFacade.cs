using Persistence.ReadModels;

namespace Persistence
{
    public interface IPersistenceFacade
    {
        
    }

    public interface IStorageItemRepository
    {
        Task<IEnumerable<StorageItemRead>> GetStorageItems(object query);
    }

    public interface IStorageRepository
    {
        //Task<StorageRead?> GetStorageByQuery(object query);
        Task<StorageRead?> GetStorageWithSpace();
        Task<StorageRead?> GetStorageWithProduct(Guid productId);
        Task<StorageRead?> GetStorageById(Guid storageId);
    }

    public interface IProductStorageReservationRepository
    {
        Task<bool> RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId);
        Task<Guid> RetrieveStorageReservation(string forExternalReference, Guid productId);
        Task RemoveStorageReservation(string forExternalReference, Guid productId);
    }


}
