using FCSP.Models.Context;
using FCSP.Models.Entities;
using FCSP.Repositories.Implementations;

namespace FCSP.Repositories
{
    public class TemplateRepository : GenericRepository<CustomShoeDesignTemplate>, ITemplateRepository
    {
        public TemplateRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
