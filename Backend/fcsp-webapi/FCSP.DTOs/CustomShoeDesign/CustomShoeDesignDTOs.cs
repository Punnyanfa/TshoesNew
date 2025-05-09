using FCSP.Common.Enums;
using FCSP.DTOs.Size;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FCSP.DTOs.CustomShoeDesign
{
    public class GetCustomShoeDesignByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetAllPublicCustomShoeDesignsResponse
    {
        public IEnumerable<GetSimpleCustomShoeDesignResponse>? Designs { get; set; }
    }

    public class GetCustomShoeDesignsByUserIdRequest 
    {
        public long UserId { get; set; }
    }

    public class GetListCustomShoeDesignsResponse 
    {
        public IEnumerable<GetSimpleCustomShoeDesignResponse>? Designs { get; set; }
    }

    public class GetSimpleCustomShoeDesignResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? PreviewImageUrl { get; set; }
        public float? Rating { get; set; }
        public int? RatingCount { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
        public CustomShoeDesignStatus? Status { get; set; }
        public string? Gender { get; set; }
    }

    public class GetCustomShoeDesignByIdResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? TemplateUrl { get; set; }
        public string? DesignData { get; set; }
        public float? Price { get; set; }
        public IEnumerable<ShoeSizes>? Sizes { get; set; }
        public IEnumerable<string>? TexturesUrls { get; set; }
        public IEnumerable<GetCustomShoeDesignServiceByIdResponse>? Services { get; set; }
    }

    public class GetCustomShoeDesignServiceByIdResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
    }

    public class AddCustomShoeDesignRequest
    {
        public long? UserId { get; set; }
        public long? CustomShoeDesignTemplateId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? DesignData { get; set; }
        public int? DesignerMarkup { get; set; } = 0;
        public List<IFormFile> CustomShoeDesignPreviewImages { get; set; } = new List<IFormFile>();
        public List<long>? TextureIds { get; set; }
        public List<long>? ServiceIds { get; set; }
    }

    public class AddCustomShoeDesignResponse
    {
        public bool Success { get; set; }
    }

    public class UpdateCustomShoeDesignStatusRequest
    {
        public long Id { get; set; }
        public CustomShoeDesignStatus Status { get; set; }
    }

    public class UpdateCustomShoeDesignStatusResponse
    {
        public bool Success { get; set; }
    }
    
    public class UpdateCustomShoeDesignRequest
    {
        public long Id { get; set; }
        public long CustomShoeDesignTemplateId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DesignData { get; set; }
        public float? DesignerMarkup { get; set; } = 0;
        public List<long>? TextureIds { get; set; }
        public List<long>? ServiceIds { get; set; }
    }

    public class UpdateCustomShoeDesignResponse
    {
        public bool Success { get; set; }
    }

    public class DeleteCustomShoeDesignRequest
    {
        public long Id { get; set; }
    }

    public class DeleteCustomShoeDesignResponse
    {
        public bool Success { get; set; }
    }
} 