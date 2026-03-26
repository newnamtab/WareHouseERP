using Persistence;
using Persistence.Repositories;

namespace StorageManagement
{
    public interface IProductStorageReservationManager
    {
        bool RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId);
        Guid RetrieveStorageReservation(string forExternalReference, Guid productId);
        void RemoveStorageReservation(string forExternalReference, Guid productId);
    }

    internal class StorageReservationManager
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IStorageItemWriteRepository _storageItemWriteRepository;
        private readonly IProductStorageReservationManager _productStorageReservationManager;

        public StorageReservationManager(IStorageRepository storageRepository, IStorageItemWriteRepository storageItemWriteRepository, IProductStorageReservationManager productStorageReservationManager)
        {
            _storageRepository = storageRepository;
            _storageItemWriteRepository = storageItemWriteRepository;
            _productStorageReservationManager = productStorageReservationManager;
        }

        public async Task<bool> ProductIn(AddItemInformation addItem)
        {
            var relevantStorage = await _storageRepository.GetStorageWithSpace();
            if (relevantStorage != null)
            {
              return await _storageItemWriteRepository.AddItemToStorage(addItem, relevantStorage.Id);
            }
            return false;
        }

        public async Task<bool> ReserveProduct( string forExternaleReference, Guid productId)
        {
            var relevantStorage = await _storageRepository.GetStorageWithProduct(productId);
            if (relevantStorage != null)
            {
                return _productStorageReservationManager.RecordStorageReservation(forExternaleReference, relevantStorage.Id, productId);
            }
            return false;
        }
        public async Task<Guid> ProductOut( string forExternaleReference, Guid productId )
        {
            var relevantStorageId = await RetrieveStorageReservation( forExternaleReference, productId );
            if (relevantStorageId.HasValue)
            {
                var removedItemId = await _storageItemWriteRepository.PickItemFromStorage(productId, relevantStorageId.Value);
                if (removedItemId != Guid.Empty)
                {
                    _productStorageReservationManager.RemoveStorageReservation(forExternaleReference, productId);
                    return removedItemId;
                }
            }
            return Guid.Empty;
        }
        private async Task<Guid?> RetrieveStorageReservation(string forExternalReference, Guid productId )
        {
            var storageId = _productStorageReservationManager.RetrieveStorageReservation(forExternalReference, productId);
            return (await _storageRepository.GetStorageById(storageId))?.Id;
        }
    }
}
