using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FcspDbContext context) : base(context)
        {
        }

        // Implement any custom repository methods here
    }
}