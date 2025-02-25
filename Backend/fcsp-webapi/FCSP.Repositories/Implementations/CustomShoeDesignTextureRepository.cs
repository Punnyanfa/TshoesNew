using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignTextureRepository : GenericRepository<CustomShoeDesignTexture>, ICustomShoeDesignTextureRepository
    {
        public CustomShoeDesignTextureRepository(FcspDbContext context) : base(context)
        {
        }

        // Implement any custom repository methods here
    }
}