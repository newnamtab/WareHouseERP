namespace StorageManagement
{
    public interface IStorageManager
    {
        IStorage? GetStorageWithSpace();
        IStorage? GetStorageWithProduct(Guid productId);
        IStorage? GetStorageById(Guid storageId);
    }
    public interface IProductStorageReservationManager
    {
        bool RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId);
        Guid RetrieveStorageReservation(string forExternalReference, Guid productId);
        void RemoveStorageReservation(string forExternalReference, Guid productId);
    }

    public class StorageReservationManager
    {
        private readonly IStorageManager _storageManager;
        private readonly IProductStorageReservationManager _productStorageReservationManager;

        public StorageReservationManager(IStorageManager storageManager, IProductStorageReservationManager productStorageReservationManager)
        {
            _storageManager = storageManager;
            _productStorageReservationManager = productStorageReservationManager;
        }

        public bool ProductIn(Guid productId)
        {
            var relevantStorage = _storageManager.GetStorageWithSpace();
            if (relevantStorage != null)
            {
                return relevantStorage.AddItem(productId);
            }
            return false;
        }

        public bool ReserveProduct( string forExternaleReference, Guid productId)
        {
            var relevantStorage = _storageManager.GetStorageWithProduct(productId);
            if (relevantStorage != null)
            {
                return _productStorageReservationManager.RecordStorageReservation(forExternaleReference, relevantStorage.Id, productId);
            }
            return false;
        }
        public Guid ProductOut( string forExternaleReference, Guid productId )
        {
            var relevantStorage = RetrieveStorageReservation( forExternaleReference, productId );
            if (relevantStorage != null)
            {
                var itemRemoved = relevantStorage.PickItem(productId);
                if (itemRemoved.ItemId != Guid.Empty)
                {
                    _productStorageReservationManager.RemoveStorageReservation(forExternaleReference, productId);
                    return itemRemoved.ItemId;
                }
            }
            return Guid.Empty;
        }
        public IStorage? RetrieveStorageReservation(string forExternalReference, Guid productId )
        {
            var storageId = _productStorageReservationManager.RetrieveStorageReservation(forExternalReference, productId);
            return _storageManager.GetStorageById(storageId);
        }
    }
}
