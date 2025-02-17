using FCSP.Models.Context;
using FCSP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.Repositories
{
    public class TemplateRepository : GenericRepository<CustomShoeDesignTemplate>, ITemplateRepository
    {
        public TemplateRepository(FcspDbContext context) : base(context)
        {
        }
    }
}
