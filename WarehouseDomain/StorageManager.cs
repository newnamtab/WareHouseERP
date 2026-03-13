using System.Collections.Concurrent;

namespace StorageManagement
{
    public interface IStorageInitializer
    {
        IEnumerable<Storage> AvailableStorages();
    }

    internal class StorageManager
    {
        private readonly IEnumerable<Storage> _storages;
        private readonly ConcurrentDictionary<string,  Dictionary<Guid, Guid>> _productStorageMap;

        private StorageManager(IEnumerable<Storage> storages)
        {
            _storages = storages;
            _productStorageMap = new ConcurrentDictionary<string, Dictionary<Guid, Guid>>();
        }
        public static StorageManager Initialize(IStorageInitializer storageFactory)
        {
            return new StorageManager( storageFactory.AvailableStorages() );
        }

        public bool ReserveProduct(Guid productId, string forExternaleReference)
        {
            var relevantStorage = _storages.FirstOrDefault(storage => storage.HasProductAvailable(productId));
            if (relevantStorage != null)
            {
                return RecordStorageReservation(forExternaleReference, relevantStorage.Id, productId);
            }
            return false;
        }
        private bool RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId)
        {
            _productStorageMap.AddOrUpdate(forExternalReference,
                                           new Dictionary<Guid, Guid>() { { storeageId, productId } },
                                           (key, existingStorage) => {
                                                                         existingStorage.TryAdd(storeageId, productId);
                                                                         return existingStorage;
                                                                     }
                                            );
            return true;
        }

        public Guid ProductOut(string forExternaleReference, Guid productId)
        {
            var relevantStorage = RetrieveStorageReservation( forExternaleReference, productId );
            if (relevantStorage != null)
            {
                var itemRemoved = relevantStorage.RemoveItem(productId);
                if (itemRemoved.ItemId != Guid.Empty)
                {
                    RemoveStorageReservation(forExternaleReference, productId);
                    return itemRemoved.ItemId;
                }
            }
            return Guid.Empty;
        }
        private void RemoveStorageReservation(string forExternalReference, Guid productId)
        {
            if (_productStorageMap.TryGetValue(forExternalReference, out var storageReservations) &&
                storageReservations.TryGetValue(productId, out var storageId))
            {
                storageReservations.Remove(productId);
            }
        }
        private Storage RetrieveStorageReservation(string forExternalReference, Guid productId)
        {
            if (_productStorageMap.TryGetValue(forExternalReference, out var storageReservations) &&
                storageReservations.TryGetValue(productId, out var storageId))
            {
                var relevantStorage = _storages.FirstOrDefault(storage => storage.Id == storageId);
                if (relevantStorage != null)
                {
                    return relevantStorage;
                }
            }
            return null;
        }

        public bool ProductIn(Guid productId)
        {
            var relevantStorage = _storages.FirstOrDefault(storage => storage.HasStorageSpace(productId));
            if (relevantStorage != null)
            {
                return relevantStorage.AddItem(productId);
            }
            return false;
        }
    }
}
