using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories
{
    public class TextureRepository : GenericRepository<Texture>, ITextureRepository
    {
        public TextureRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
