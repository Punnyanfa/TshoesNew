using System.Collections.Generic;

namespace FCSP.DTOs.CustomShoeDesign
{
    public class GetCustomShoeDesignByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetCustomShoeDesignByIdResponse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? CustomShoeDesignTemplateId { get; set; }
        public string? DesignData { get; set; }
        public string? Preview3DFileUrl { get; set; }
        public float? Price { get; set; }
        public List<long>? TextureIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddCustomShoeDesignRequest
    {
        public long? UserId { get; set; }
        public long? CustomShoeDesignTemplateId { get; set; }
        public string? DesignData { get; set; }
        public string? Preview3DFileUrl { get; set; }
        public float? Price { get; set; }
        public List<long>? TextureIds { get; set; }
    }

    public class AddCustomShoeDesignResponse
    {
        public long Id { get; set; }
        public string? Preview3DFileUrl { get; set; }
        public float? Price { get; set; }
    }

    public class UpdateCustomShoeDesignRequest
    {
        public long Id { get; set; }
        public long? CustomShoeDesignTemplateId { get; set; }
        public string? DesignData { get; set; }
        public string? Preview3DFileUrl { get; set; }
        public float? Price { get; set; }
        public List<long>? TextureIds { get; set; }
    }

    public class UpdateCustomShoeDesignResponse
    {
        public long Id { get; set; }
        public string? Preview3DFileUrl { get; set; }
        public float? Price { get; set; }
    }

    public class DeleteCustomShoeDesignRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCustomShoeDesignResponse
    {
        public bool Success { get; set; }
    }

    public class GetCustomShoeDesignsByUserRequest
    {
        public long UserId { get; set; }
    }
    
    public class GetDesignsByUserRequest 
    {
        public long UserId { get; set; }
    }
} 