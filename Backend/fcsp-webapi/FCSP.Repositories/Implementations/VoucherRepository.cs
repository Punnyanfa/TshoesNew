using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<Voucher> GetVoucherByOrderIdAsync(long orderId)
        {
            return await _context.Vouchers
                .Include(v => v.Orders)
                .FirstOrDefaultAsync(v => v.Orders.Any(o => o.Id == orderId));
        }

        public async Task UpdateExpiredVouchersAsync()
        {
            var expiredVouchers = await _context.Vouchers
                .Where(v => v.ExpirationDate < DateTime.UtcNow && v.Status == (int)FCSP.Common.Enums.VoucherStatus.Active)
                .ToListAsync();

            foreach (var voucher in expiredVouchers)
            {
                voucher.Status = (int)FCSP.Common.Enums.VoucherStatus.Expired;
                voucher.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}