using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class TextureRepository : GenericRepository<Texture>, ITextureRepository
    {
        public TextureRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Texture>> GetTexturesByUserIdAsync(long userId)
        {
            return await Entities
                .Where(t => t.UserId == userId && t.IsDeleted == 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Texture>> GetAvailableTexturesAsync()
        {
            // Return active textures (Status = 1 means active, IsDeleted = 0 means not deleted)
            return await Entities
                .Where(t => t.Status == 1 && t.IsDeleted == 0)
                .OrderBy(t => t.Id) // Order by Id as there's no Price property
                .ToListAsync();
        }
    }
} 