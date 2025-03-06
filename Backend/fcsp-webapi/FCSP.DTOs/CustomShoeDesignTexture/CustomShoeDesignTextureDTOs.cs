namespace FCSP.DTOs.CustomShoeDesignTexture
{
    public class GetCustomShoeDesignTextureByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetCustomShoeDesignTextureByIdResponse
    {
        public long Id { get; set; }
        public long CustomShoeDesignId { get; set; }
        public long TextureId { get; set; }
        public string Position { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddCustomShoeDesignTextureRequest
    {
        public long CustomShoeDesignId { get; set; }
        public long TextureId { get; set; }
        public string Position { get; set; } = null!;
    }

    public class AddCustomShoeDesignTextureResponse
    {
        public long Id { get; set; }
        public string Position { get; set; } = null!;
    }

    public class UpdateCustomShoeDesignTextureRequest
    {
        public long Id { get; set; }
        public long TextureId { get; set; }
        public string Position { get; set; } = null!;
    }

    public class UpdateCustomShoeDesignTextureResponse
    {
        public long Id { get; set; }
        public string Position { get; set; } = null!;
    }

    public class DeleteCustomShoeDesignTextureRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCustomShoeDesignTextureResponse
    {
        public bool Success { get; set; }
    }

    public class GetTexturesByDesignRequest
    {
        public long CustomShoeDesignId { get; set; }
    }
} 