using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<Cart> GetCartByUserIdAsync(long userId)
        {
            return await Entities.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }

        public async Task<Cart> GetCartWithItemsByUserIdAsync(long userId)
        {
            return await Entities
                .Include(c => c.CartItems.Where(ci => !ci.IsDeleted))
                .ThenInclude(ci => ci.CustomShoeDesign)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }
    }
} 