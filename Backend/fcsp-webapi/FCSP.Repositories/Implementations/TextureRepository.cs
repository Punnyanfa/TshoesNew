using FCSP.Common.Enums;
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
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Texture>> GetAvailableTexturesAsync()
        {
            return await Entities
                .Where(t => t.Status != TextureStatus.Archived && !t.IsDeleted)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }
    }
}