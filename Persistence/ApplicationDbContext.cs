using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal interface IApplicationDbContext
    {
        DbSet<Entities.Storage> Storages { get; set; }
        DbSet<Entities.StorageItem> StorageItems { get; set; }
        DbSet<Entities.StorageItemCategory> StorageItemCategories { get; set; }
        DbSet<Entities.ItemStorageReservation> ItemStorageReservations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Entities.Storage> Storages { get; set; }
        public DbSet<Entities.StorageItem> StorageItems { get; set; }
        public DbSet<Entities.StorageItemCategory> StorageItemCategories { get; set; }

        public DbSet<Entities.ItemStorageReservation> ItemStorageReservations { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.StorageItem>()
                .HasMany(si => si.StorageItemCategories)
                .WithMany();

            modelBuilder.Entity<Entities.Storage>()
                .HasMany(s => s.StorageItems)
                .WithOne(i => i.Storage)
                .HasForeignKey(i => i.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Entities.ItemStorageReservation>()
                .Property(x=>x.ProductId)
                .IsRequired();
            modelBuilder
               .Entity<Entities.ItemStorageReservation>()
               .Property(x => x.StorageId)
               .IsRequired();
            
            modelBuilder
               .Entity<Entities.ItemStorageReservation>()
               .Property(x => x.ExternalReference)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
