namespace FCSP.DTOs.DesignPreview
{
    public class GetDesignPreviewByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetDesignPreviewByIdResponse
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public string? PreviewImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetPreviewsByDesignRequest
    {
        public long CustomShoeDesignId { get; set; }
    }

    public class AddDesignPreviewRequest
    {
        public long CustomShoeDesignId { get; set; }
        public string? PreviewImageUrl { get; set; }
    }

    public class AddDesignPreviewResponse
    {
        public long DesignPreviewId { get; set; }
    }

    public class UpdateDesignPreviewRequest
    {
        public long Id { get; set; }
        public string? PreviewImageUrl { get; set; }
    }

    public class UpdateDesignPreviewResponse
    {
        public long DesignPreviewId { get; set; }
    }

    public class DeleteDesignPreviewRequest
    {
        public long Id { get; set; }
    }

    public class DeleteDesignPreviewResponse
    {
        public bool Success { get; set; }
    }
} 