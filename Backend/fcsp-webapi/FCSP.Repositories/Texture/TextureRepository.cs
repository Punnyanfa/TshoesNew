using FCSP.Models.Context;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Texture
{
    public class TextureRepository : GenericRepository<Models.Entities.Texture>, ITextureRepository
    {
        public TextureRepository(FcspDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Entities.Texture>> GetTexturesByUserIdAsync(long userId)
        {
            return await Entities
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Entities.Texture>> GetAvailableTexturesAsync()
        {
            return await Entities
                .Where(t => t.Price != null && t.Price > 0)
                .OrderBy(t => t.Price)
                .ToListAsync();
        }
    }
} 