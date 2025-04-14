using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(long userId);
        Task<Cart> GetCartWithItemsByUserIdAsync(long userId);
    }
}