using FCSP.Data;
using FCSP.Models.Entities;

namespace FCSP.Repositories.CustomShoeDesignTexture
{
    public class CustomShoeDesignTextureRepository : BaseRepository<CustomShoeDesignTexture>, ICustomShoeDesignTextureRepository
    {
        public CustomShoeDesignTextureRepository(FCDbContext context) : base(context)
        {
        }
        
        // Implement any custom repository methods here
    }
} 