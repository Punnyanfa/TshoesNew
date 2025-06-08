using FCSP.Common.Enums;
using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FCSP.Common.Utils;

namespace FCSP.Repositories.Implementations
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(FcspDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Voucher>> GetNonExpiredVouchersAsync()
        {
            return await _context.Vouchers
                 .Where(v => v.ExpirationDate >= DateTimeUtils.GetCurrentGmtPlus7() && v.Status == (int)VoucherStatus.Active && !v.IsDeleted)
                 .Include(v => v.Orders)
                 .ToListAsync();
        }
        public async Task<IEnumerable<Voucher>> GetAllVoucherAsync()
        {
            return await _context.Vouchers
                .Where(v => !v.IsDeleted)
                .Include(v => v.Orders)
                .ToListAsync();
        }

        public async Task<Voucher> GetVoucherByOrderIdAsync(long orderId)
        {
            return await _context.Vouchers
                .Include(v => v.Orders)
                .FirstOrDefaultAsync(v => v.Orders.Any(o => o.Id == orderId) && !v.IsDeleted);
        }

        public async Task<int> UpdateExpiredVouchersAsync()
        {
            var now = DateTimeUtils.GetCurrentGmtPlus7();
            var vouchersToUpdate = await _context.Vouchers
                .Where(v => v.ExpirationDate < now && v.Status == (int)VoucherStatus.Active)
                .ToListAsync();

            foreach (var voucher in vouchersToUpdate)
            {
                voucher.Status = (int)VoucherStatus.Expired;
                voucher.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
            }

             await _context.SaveChangesAsync();
            return vouchersToUpdate.Count; 
        }
    }
}