using FCSP.Models.Context;
using FCSP.Models.Entities;

namespace FCSP.Repositories
{
    public class TemplateRepository : GenericRepository<CustomShoeDesignTemplate>, ITemplateRepository
    {
        public TemplateRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
