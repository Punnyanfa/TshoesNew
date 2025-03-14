namespace FCSP.Models.Entities;

public class CustomShoeDesign : BaseEntity
{
    public long UserId { get; set; }
    public long CustomShoeDesignTemplateId { get; set; }
    public string? DesignData { get; set; }
    public string? Size { get; set; }
    public int Status { get; set; }
    public float DesignerMarkup { get; set; }
    public float TotalAmount { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual CustomShoeDesignTemplate CustomShoeDesignTemplate { get; set; } = null!;
    public virtual ICollection<CustomShoeDesignTexture> CustomShoeDesignTextures { get; } = [];
    public virtual ICollection<CustomShoeDesignImage> CustomShoeDesignImages { get; } = [];
    public virtual ICollection<DesignPreview> DesignPreviews { get; } = [];
    public virtual ICollection<Rating> Ratings { get; } = [];
    public virtual ICollection<UserActivity> ViewedActivities { get; } = [];
    public virtual ICollection<UserRecommendation> Recommendations { get; } = [];
    public virtual ICollection<DesignService> DesignServices { get; } = [];
    public virtual ICollection<OrderDetail> OrderDetails { get; } = [];
    public virtual ICollection<ReturnedCustomShoe> ReturnedCustomShoes { get; } = [];
}
