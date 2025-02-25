using FCSP.Data;
using FCSP.Models.Entities;

namespace FCSP.Repositories.Order
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(FCDbContext context) : base(context)
        {
        }
        
        // Implement any custom repository methods here
    }
} 