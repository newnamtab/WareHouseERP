namespace StorageManagement
{
    internal class Storage
    {
        public Guid Id { get; private set; }

        public Storage(Guid storageId)
        {
            Id = storageId;
        }
    }
}
