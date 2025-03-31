using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task<IEnumerable<long>> GetTopFiveBestSellingDesignsAsync();
    }
}