using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IList<Transaction>> GetByReceiverIdAsync(long receiverId);
        Task<IList<Transaction>> GetByOrderDetailIdAsync(long orderDetailId);
        Task<IList<Transaction>> GetByPaymentIdAsync(long paymentId);
    }
} 