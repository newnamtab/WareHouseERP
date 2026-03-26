using System.Collections.Concurrent;

namespace StorageManagement
{
    internal class ProductStorageReservationManager : IProductStorageReservationManager
    {
        private readonly ConcurrentDictionary<string, Dictionary<Guid, Guid>> _productStorageReservationMap;

        public ProductStorageReservationManager()
        {
            _productStorageReservationMap = new ConcurrentDictionary<string, Dictionary<Guid, Guid>>();
        }
        public bool RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId)
        {
            _productStorageReservationMap.AddOrUpdate(forExternalReference,
                                           new Dictionary<Guid, Guid>() { { storeageId, productId } },
                                           (key, existingStorage) => {
                                               existingStorage.TryAdd(storeageId, productId);
                                               return existingStorage;
                                           }
                                            );
            return true;
        }

        public Guid RetrieveStorageReservation(string forExternalReference, Guid productId)
        {
            return (_productStorageReservationMap.TryGetValue(forExternalReference, out var storageReservations)
                    && storageReservations.TryGetValue(productId, out var storageId))
                                            ? storageId
                                            : Guid.Empty;
        }

        public void RemoveStorageReservation(string forExternalReference, Guid productId)
        {
            if (_productStorageReservationMap.TryGetValue(forExternalReference, out var storageReservations) &&
                storageReservations.TryGetValue(productId, out var storageId))
            {
                storageReservations.Remove(productId);
            }
        }
    }
}
