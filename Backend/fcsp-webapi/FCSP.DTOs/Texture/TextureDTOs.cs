using System.Text.Json.Serialization;
using FCSP.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace FCSP.DTOs.Texture
{
    public class GetTextureByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetTextureByIdResponse
    {
        public string ImageUrl { get; set; } = null!;
    }

    public class GetTexturesByUserRequest
    {
        public long UserId { get; set; }
    }

    public class AddTextureRequest
    {
        public IFormFile? ImageFile { get; set; } = null!;
        public string? Prompt { get; set; } = null!;
        public long OwnerId { get; set; }
        public TextureStatus Status { get; set; } = TextureStatus.Private;
    }

    public class AddTextureResponse
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; } = null!;
    }

    public class DeleteTextureRequest
    {
        public long Id { get; set; }
    }

    public class DeleteTextureResponse
    {
        public bool Success { get; set; }
    }

    public class GetTexturesByBuyerRequest
    {
        public long BuyerId { get; set; }
    }

    public class GetTexturesByOwnerRequest
    {
        public long OwnerId { get; set; }
    }

    public class GenerateImageRequest
    {
        public string? Prompt { get; set; } = null!;
    }

    public class GenerateImageResponse
    {
        public string? ImageUrl { get; set; }
        public string? Prompt { get; set; }
    }

    public class ImageGenerationResponse
    {
        [JsonPropertyName("data")]
        public List<ImageData> Data { get; set; }
    }

    public class ImageData
    {
        [JsonPropertyName("b64_json")]
        public string B64Json { get; set; }
    }
} 