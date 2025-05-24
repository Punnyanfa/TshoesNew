using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IList<CartItem>> GetCartItemsByCartIdAsync(long cartId)
        {
            return await Entities
                .Where(ci => ci.CartId == cartId && !ci.IsDeleted)
                .Include(ci => ci.CustomShoeDesign)
                .ToListAsync();
        }

        public async Task<CartItem> GetCartItemByDesignIdAndCartIdAsync(long designId, long cartId, long sizeId)
        {
            return await Entities.FirstOrDefaultAsync(
                    ci => ci.CartId == cartId && 
                    ci.CustomShoeDesignId == designId &&
                    ci.SizeId == sizeId && 
                    !ci.IsDeleted);    
        }
            
    }
}