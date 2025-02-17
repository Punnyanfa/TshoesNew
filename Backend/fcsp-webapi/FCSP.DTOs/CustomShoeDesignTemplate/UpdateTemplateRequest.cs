using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.DTOs.CustomShoeDesignTemplate
{
    public class UpdateTemplateRequest
    {
        public long Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
