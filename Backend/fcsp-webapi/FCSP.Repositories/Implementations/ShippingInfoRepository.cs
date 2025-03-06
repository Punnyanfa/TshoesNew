using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class ShippingInfoRepository : GenericRepository<ShippingInfo>, IShippingInfoRepository
    {
        public ShippingInfoRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
