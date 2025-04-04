using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface IPaymentGatewayRepository : IGenericRepository<PaymentGateway>
    {
        Task<IEnumerable<PaymentGateway>> GetByUserIdAsync(long userId);
    }
}