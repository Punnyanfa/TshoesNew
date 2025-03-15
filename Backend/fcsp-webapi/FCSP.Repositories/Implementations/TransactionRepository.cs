using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FCSP.Repositories.Implementations
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IList<Transaction>> GetByReceiverIdAsync(long receiverId)
        {
            return await Entities
                .Where(t => t.ReceiverId == receiverId)
                .Include(t => t.Receiver)
                .Include(t => t.OrderDetail)
                .Include(t => t.Payment)
                .ToListAsync();
        }

        public async Task<IList<Transaction>> GetByOrderDetailIdAsync(long orderDetailId)
        {
            return await Entities
                .Where(t => t.OrderDetailId == orderDetailId)
                .Include(t => t.Receiver)
                .Include(t => t.OrderDetail)
                .Include(t => t.Payment)
                .ToListAsync();
        }

        public async Task<IList<Transaction>> GetByPaymentIdAsync(long paymentId)
        {
            return await Entities
                .Where(t => t.PaymentId == paymentId)
                .Include(t => t.Receiver)
                .Include(t => t.OrderDetail)
                .Include(t => t.Payment)
                .ToListAsync();
        }
    }
}