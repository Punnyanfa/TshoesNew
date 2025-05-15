using FCSP.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Repositories.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<IList<CartItem>> GetCartItemsByCartIdAsync(long cartId);
        Task<CartItem> GetCartItemByDesignIdAndCartIdAsync(long designId, long cartId);
    }
} 