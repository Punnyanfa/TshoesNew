using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class PaymentGatewayRepository : GenericRepository<PaymentGateway>, IPaymentGatewayRepository
    {
        public PaymentGatewayRepository(FcspDbContext context) : base(context)
        {
        }

        // Implement any custom repository methods here
    }
}