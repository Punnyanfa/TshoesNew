using System.ComponentModel.DataAnnotations;

namespace FCSP.DTOs.CustomShoeDesignTemplate
{
    public class GetTemplateByIdRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }
    }

    public class GetTemplateByIdResponse
    {
        [Required(ErrorMessage = "Id is required")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Model3DUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for Model3DUrl")]
        public string Model3DUrl { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "BasePrice cannot be negative")]
        public decimal BasePrice { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "CreatedAt is required")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "UpdatedAt is required")]
        public DateTime UpdatedAt { get; set; }
    }

    public class AddTemplateRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Unisex)$", ErrorMessage = "Gender must be Male, Female, or Unisex")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Color is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Color must be between 2 and 50 characters")]
        public string Color { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Model3DUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for Model3DUrl")]
        public string Model3DUrl { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "BasePrice cannot be negative")]
        public decimal BasePrice { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class AddTemplateResponse
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;
    }

    public class UpdateTemplateRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(Male|Female|Unisex)$", ErrorMessage = "Gender must be Male, Female, or Unisex")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Color is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Color must be between 2 and 50 characters")]
        public string Color { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Model3DUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for Model3DUrl")]
        public string Model3DUrl { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "BasePrice cannot be negative")]
        public decimal BasePrice { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class UpdateTemplateResponse
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;
    }

    public class DeleteTemplateRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }
    }

    public class DeleteTemplateResponse
    {
        public bool Success { get; set; }
    }

    public class GetCustomShoeDesignIdsByTemplateRequest
    {
        [Required(ErrorMessage = "TemplateId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "TemplateId must be greater than 0")]
        public long TemplateId { get; set; }
    }

    public class GetCustomShoeDesignIdsByTemplateResponse
    {
        [Required(ErrorMessage = "CustomShoeDesignIds is required")]
        public IEnumerable<long> CustomShoeDesignIds { get; set; } = [];
    }

    public class SearchTemplatesRequest
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string? Name { get; set; }

        [RegularExpression("^(Male|Female|Unisex)$", ErrorMessage = "Gender must be Male, Female, or Unisex")]
        public string? Gender { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Color must be between 2 and 50 characters")]
        public string? Color { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MinPrice cannot be negative")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MaxPrice cannot be negative")]
        public decimal? MaxPrice { get; set; }

        public bool? IsAvailable { get; set; } = true; // Mặc định chỉ lấy các template available
    }

    public class RestoreTemplateRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }
    }

    public class RestoreTemplateResponse
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        public bool Success { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; } = null!;
    }

    public class GetTemplateStatsRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }
    }

    public class GetTemplateStatsResponse
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "CustomShoeDesignCount cannot be negative")]
        public int CustomShoeDesignCount { get; set; }

        [Required(ErrorMessage = "CreatedAt is required")]
        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class GetPopularTemplatesRequest
    {
        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100")]
        public int Limit { get; set; } = 10;
    }

    public class GetPopularTemplatesResponse
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Id must be greater than 0")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "PreviewImageUrl is required")]
        [Url(ErrorMessage = "Invalid URL format for PreviewImageUrl")]
        public string PreviewImageUrl { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "CustomShoeDesignCount cannot be negative")]
        public int CustomShoeDesignCount { get; set; }
    }
}