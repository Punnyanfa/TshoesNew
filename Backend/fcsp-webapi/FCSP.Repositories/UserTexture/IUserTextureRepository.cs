using FCSP.Models.Entities;

namespace FCSP.Repositories.UserTexture
{
    public interface IUserTextureRepository : IGenericRepository<Models.Entities.UserTexture>
    {
        Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByOwnerIdAsync(long ownerId);
        Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByBuyerIdAsync(long buyerId);
        Task<IEnumerable<Models.Entities.UserTexture>> GetTexturesByTextureIdAsync(long textureId);
    }
} 