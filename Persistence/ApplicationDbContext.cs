using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Entities.StorageItem> StorageItems { get; set; }
        public DbSet<Entities.Storage> Storages { get; set; }
        public DbSet<Entities.ProductStorageReservation> ProductStorageReservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.StorageItem>();

            modelBuilder.Entity<Entities.Storage>()
                .HasMany(s => s.StorageItems)
                .WithOne(i => i.Storage)
                .HasForeignKey(i => i.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Entities.ProductStorageReservation>()
                .Property(x=>x.ProductId)
                .IsRequired();
            modelBuilder
               .Entity<Entities.ProductStorageReservation>()
               .Property(x => x.StorageId)
               .IsRequired();
            
            modelBuilder
               .Entity<Entities.ProductStorageReservation>()
               .Property(x => x.ExternalReference)
               .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
