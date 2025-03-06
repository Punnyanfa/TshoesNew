using System.Collections.Generic;
namespace FCSP.Models.Entities;

public class Posts : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int IsDeleted { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PostsComments> PostsComments { get; } = [];
} 