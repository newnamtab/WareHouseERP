using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using StorageManagement;

namespace Persistence.Repositories
{
    internal class ProductStorageReservationProvider : IProductStorageReservationProvider
    {
        private readonly IApplicationDbContext _context;

        public ProductStorageReservationProvider(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RecordStorageReservation(string forExternalReference, Guid storeageId, Guid productId)
        {
            if (_context.ItemStorageReservations.Add(new ItemStorageReservation
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
            await _context.ItemStorageReservations
                          .Where(prs => prs.ProductId == productId
                                     && prs.ExternalReference == forExternalReference )
                          .ExecuteDeleteAsync();
        }

        public async Task<Guid> RetrieveStorageReservation(string forExternalReference, Guid productId)
        {
            var reservation = await _context.ItemStorageReservations
                                            .FirstOrDefaultAsync(r => r.ExternalReference == forExternalReference
                                                                   && r.ProductId == productId);
            return reservation != null
                ? reservation.StorageId
                : Guid.Empty;
        }
    }
}
