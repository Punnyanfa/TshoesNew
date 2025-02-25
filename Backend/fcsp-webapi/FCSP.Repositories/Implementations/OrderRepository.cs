using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(FcspDbContext context) : base(context)
        {
        }

        // Implement any custom repository methods here
    }
}