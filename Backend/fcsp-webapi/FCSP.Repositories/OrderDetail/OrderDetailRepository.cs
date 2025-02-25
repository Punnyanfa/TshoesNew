using FCSP.Data;
using FCSP.Models.Entities;

namespace FCSP.Repositories.OrderDetail
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FCDbContext context) : base(context)
        {
        }
        
        // Implement any custom repository methods here
    }
} 