using FCSP.Models.Entities;

namespace FCSP.Repositories.Texture
{
    public interface ITextureRepository : IGenericRepository<Models.Entities.Texture>
    {
        Task<IEnumerable<Models.Entities.Texture>> GetTexturesByUserIdAsync(long userId);
        Task<IEnumerable<Models.Entities.Texture>> GetAvailableTexturesAsync();
    }
} 