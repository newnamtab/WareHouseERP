namespace Persistence.Entities
{
    internal class Storage
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public ICollection<StorageItem> StorageItems { get; set; }
    }
}
