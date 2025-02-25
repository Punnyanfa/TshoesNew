using FCSP.Data;
using FCSP.Models.Entities;

namespace FCSP.Repositories.Payment
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(FCDbContext context) : base(context)
        {
        }
        
        // Implement any custom repository methods here
    }
} 