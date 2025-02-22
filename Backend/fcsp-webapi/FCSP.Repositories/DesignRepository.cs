using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories
{
    public class DesignRepository : GenericRepository<CustomShoeDesign>, IDesignRepository
    {
        public DesignRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
