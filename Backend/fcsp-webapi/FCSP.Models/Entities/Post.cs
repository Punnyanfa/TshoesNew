using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.Models.Entities
{
    public class Post : BaseEntity
    {
        public long? UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual IEnumerable<PostComment> PostComments { get; } = [];
    }
}
