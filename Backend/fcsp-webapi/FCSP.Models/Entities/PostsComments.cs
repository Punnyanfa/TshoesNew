using System.ComponentModel.DataAnnotations;
namespace FCSP.Models.Entities;

public class PostsComments : BaseEntity
{
    public long UserId { get; set; }
    public long PostsId { get; set; }  // Changed from PostId
    public string Comment { get; set; } = null!;
    public int IsDeleted { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Posts Posts { get; set; } = null!;  // Changed from Post
} 