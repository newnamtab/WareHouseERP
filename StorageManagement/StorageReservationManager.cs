namespace StorageManagement
{
    internal class StorageReservationManager
    {
        private readonly IStorageProvider _storageRepository;
        private readonly IStorageItemProvider _storageItemWriteRepository;
        private readonly IProductStorageReservationProvider _productStorageReservationRepository;

        public StorageReservationManager(IStorageProvider storageRepository, IStorageItemProvider storageItemWriteRepository, IProductStorageReservationProvider productStorageReservationRepository)
        {
            _storageRepository = storageRepository;
            _storageItemWriteRepository = storageItemWriteRepository;
            _productStorageReservationRepository = productStorageReservationRepository;
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
                return await _productStorageReservationRepository.RecordStorageReservation(forExternaleReference, relevantStorage.Id, productId);
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
                    await _productStorageReservationRepository.RemoveStorageReservation(forExternaleReference, productId);
                    return removedItemId;
                }
            }
            return Guid.Empty;
        }
        private async Task<Guid?> RetrieveStorageReservation(string forExternalReference, Guid productId )
        {
            var storageId = await _productStorageReservationRepository.RetrieveStorageReservation(forExternalReference, productId);
            return (await _storageRepository.GetStorageById(storageId))?.Id;
        }
    }
}
