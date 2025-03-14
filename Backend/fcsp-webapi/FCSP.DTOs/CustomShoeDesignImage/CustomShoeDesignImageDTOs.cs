namespace FCSP.DTOs.CustomShoeDesignImage
{
    public class GetCustomShoeDesignImageRequest
    {
        public long Id { get; set; }
    }

    public class GetCustomShoeDesignImageResponse
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public long TextureId { get; set; }
        public string TextureUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetCustomShoeDesignImagesByDesignRequest
    {
        public long CustomShoeDesignId { get; set; }
    }

    public class AddCustomShoeDesignImageRequest
    {
        public long CustomShoeDesignId { get; set; }
        public long TextureId { get; set; }
    }

    public class AddCustomShoeDesignImageResponse
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public long TextureId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateCustomShoeDesignImageRequest
    {
        public long Id { get; set; }
        public long TextureId { get; set; }
    }

    public class UpdateCustomShoeDesignImageResponse
    {
        public long Id { get; set; }
        public long TextureId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class DeleteCustomShoeDesignImageRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCustomShoeDesignImageResponse
    {
        public bool Success { get; set; }
    }
} 