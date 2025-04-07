using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByOrderIdAsync(long orderId);
    }
}