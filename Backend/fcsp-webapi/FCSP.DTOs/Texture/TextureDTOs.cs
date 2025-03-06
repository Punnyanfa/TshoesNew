using System.Text.Json.Serialization;

namespace FCSP.DTOs.Texture
{
    public class GetTextureByIdRequest
    {
        public long Id { get; set; }
    }

    public class TogetherAIConfig
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class GetTextureByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public long OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GetTexturesByUserRequest
    {
        public long UserId { get; set; }
    }

    public class AddTextureRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public long OwnerId { get; set; }
    }

    public class AddTextureResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }

    public class UpdateTextureRequest
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }

    public class UpdateTextureResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
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
        public string Prompt { get; set; } = null!;
        public string? PreferredModel { get; set; }
    }

    public class GenerateImageResponse
    {
        public bool Success { get; set; }
        public string? ImagePath { get; set; }
        public string? FileName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Prompt { get; set; }
        public string? ModelUsed { get; set; }
        public string? ErrorMessage { get; set; }
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