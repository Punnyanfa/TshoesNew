using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        Task<Voucher> GetVoucherByOrderIdAsync(long orderId);
        Task UpdateExpiredVouchersAsync();
    }
}