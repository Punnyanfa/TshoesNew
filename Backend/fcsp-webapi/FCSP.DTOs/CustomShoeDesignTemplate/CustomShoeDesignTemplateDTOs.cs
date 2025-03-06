namespace FCSP.DTOs.CustomShoeDesignTemplate
{
    public class GetTemplateByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetTemplateByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PreviewImageUrl { get; set; } = null!;
        public string Model3DUrl { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddTemplateRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PreviewImageUrl { get; set; } = null!;
        public string Model3DUrl { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class AddTemplateResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string PreviewImageUrl { get; set; } = null!;
    }

    public class UpdateTemplateRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PreviewImageUrl { get; set; } = null!;
        public string Model3DUrl { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class UpdateTemplateResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string PreviewImageUrl { get; set; } = null!;
    }

    public class DeleteTemplateRequest
    {
        public long Id { get; set; }
    }

    public class DeleteTemplateResponse
    {
        public bool Success { get; set; }
    }
} 