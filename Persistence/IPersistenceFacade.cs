using Persistence.ReadModels;

namespace Persistence
{
    public interface IStorageRepository
    {
        //Task<IEnumerable<StorageRead>> GetStoragesByQuery(object query);
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
