using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Repositories.Implementations
{
    public class TemplateRepository : GenericRepository<CustomShoeDesignTemplate>, ICustomShoeDesignTemplateRepository
    {
        public TemplateRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
