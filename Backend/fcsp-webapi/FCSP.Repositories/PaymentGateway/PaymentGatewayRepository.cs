using FCSP.Data;
using FCSP.Models.Entities;

namespace FCSP.Repositories.PaymentGateway
{
    public class PaymentGatewayRepository : BaseRepository<PaymentGateway>, IPaymentGatewayRepository
    {
        public PaymentGatewayRepository(FCDbContext context) : base(context)
        {
        }
        
        // Implement any custom repository methods here
    }
} 