using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Repositories.Implementations
{
    public class DesignServiceRepository : GenericRepository<DesignService>, IDesignServiceRepository
    {
        public DesignServiceRepository(FcspDbContext context) : base(context)
        {
        }
    }
}