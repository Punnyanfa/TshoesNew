using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Implementations;

namespace FCSP.Repositories
{
    public class ShippingInfoRepository : GenericRepository<ShippingInfo>, IShippingInfoRepository
    {
        public ShippingInfoRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
