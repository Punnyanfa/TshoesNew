using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(FcspDbContext context) : base(context)
        {
        }

        // Implement any custom repository methods here
    }
}