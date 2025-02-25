using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories.CustomShoeDesign
{
    public class CustomShoeDesignRepository : GenericRepository<Models.Entities.CustomShoeDesign>, ICustomShoeDesignRepository
    {
        public CustomShoeDesignRepository(FcspDbContext context) : base(context)
        {
        }
    }
} 