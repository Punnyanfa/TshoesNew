using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class CustomShoeDesignRepository : GenericRepository<Models.Entities.CustomShoeDesign>, ICustomShoeDesignRepository
    {
        public CustomShoeDesignRepository(FcspDbContext context) : base(context)
        {
        }
    }
}