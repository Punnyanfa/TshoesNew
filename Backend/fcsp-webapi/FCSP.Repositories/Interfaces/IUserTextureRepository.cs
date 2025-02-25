using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface IUserTextureRepository : IGenericRepository<UserTexture>
    {
        Task<IEnumerable<UserTexture>> GetTexturesByOwnerIdAsync(long ownerId);
        Task<IEnumerable<UserTexture>> GetTexturesByBuyerIdAsync(long buyerId);
        Task<IEnumerable<UserTexture>> GetTexturesByTextureIdAsync(long textureId);
    }
}