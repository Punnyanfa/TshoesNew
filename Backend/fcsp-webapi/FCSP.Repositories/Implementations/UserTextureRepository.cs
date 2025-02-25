using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class UserTextureRepository : GenericRepository<Models.Entities.UserTexture>, IUserTextureRepository
    {
        public UserTextureRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByOwnerIdAsync(long ownerId)
        {
            return await Entities.Where(ut => ut.OwnerId == ownerId).ToListAsync();
        }

        public async Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByBuyerIdAsync(long buyerId)
        {
            return await Entities.Where(ut => ut.BuyerId == buyerId).ToListAsync();
        }

        public async Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByTextureIdAsync(long textureId)
        {
            return await Entities.Where(ut => ut.TextureId == textureId).ToListAsync();
        }
    }
}