using System.ComponentModel.DataAnnotations;

namespace FCSP.Models.Entities
{
    public class UserRecommendation : BaseEntity
    {
        public long UserId { get; set; }
        public long RecommendDesignId { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual CustomShoeDesign RecommendDesign { get; set; } = null!;
    }
} 