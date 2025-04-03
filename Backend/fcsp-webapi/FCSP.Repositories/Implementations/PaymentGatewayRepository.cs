using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Repositories.Implementations
{
    public class PaymentGatewayRepository : GenericRepository<PaymentGateway>, IPaymentGatewayRepository
    {
        public PaymentGatewayRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PaymentGateway>> GetByUserIdAsync(long userId)
        {
            return await _context.Set<PaymentGateway>()
                .Where(pg => pg.UserId == userId)
                .ToListAsync();
        }

        // Implement any custom repository methods here
    }
}