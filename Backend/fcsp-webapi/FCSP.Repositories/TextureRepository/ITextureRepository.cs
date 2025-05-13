using FCSP.Models.Entities;

namespace FCSP.Repositories.Interfaces
{
    public interface ITextureRepository : IGenericRepository<Texture>
    {
        Task<IEnumerable<Texture>> GetTexturesByUserIdAsync(long userId);
        Task<IEnumerable<Texture>> GetAvailableTexturesAsync();
    }
}