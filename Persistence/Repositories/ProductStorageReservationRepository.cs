using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories
{
    internal class ProductStorageReservationRepository : IProductStorageReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductStorageReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId)
        {
            if (_context.ProductStorageReservations.Add(new ProductStorageReservation
                                                            {
                                                                ExternalReference = forExternalReference,
                                                                StorageId = storeageId,
                                                                ProductId = productId,
                                                                ReservedAt = DateTime.UtcNow
                                                            }
                ).State != EntityState.Added) return false;

            return ( await _context.SaveChangesAsync() > 0 );
        }

        public async Task RemoveStorageReservation(string forExternalReference, Guid productId)
        {
            await _context.ProductStorageReservations
                          .Where(prs => prs.ProductId == productId
                                     && prs.ExternalReference == forExternalReference )
                          .ExecuteDeleteAsync();
        }

        public async Task<Guid> RetrieveStorageReservation(string forExternalReference, Guid productId)
        {
            var reservation = await _context.ProductStorageReservations
                                            .FirstOrDefaultAsync(r => r.ExternalReference == forExternalReference
                                                                   && r.ProductId == productId);
            return reservation != null
                ? reservation.StorageId
                : Guid.Empty;
        }
    }
}
