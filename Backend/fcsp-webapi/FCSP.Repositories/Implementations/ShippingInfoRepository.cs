using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories.Implementations
{
    public class ShippingInfoRepository : GenericRepository<ShippingInfo>, IShippingInfoRepository
    {
        public ShippingInfoRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
