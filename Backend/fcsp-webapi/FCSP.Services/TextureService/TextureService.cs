using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FCSP.DTOs;
using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace FCSP.Services.TextureService;

public class TextureService : ITextureService
{
    private readonly ITextureRepository _textureRepository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string? _azureConnectionString;
    private readonly string? _azureContainerName;
    private readonly string? _promptToImproveImage;

    public TextureService(
        ITextureRepository textureRepository,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _textureRepository = textureRepository;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient("TextureService");
        _azureConnectionString = _configuration["AzureStorage:ConnectionString"];
        _azureContainerName = _configuration["AzureStorage:ContainerName"];
        _promptToImproveImage = _configuration["ImageStorage:PromptToImproveImage"];
    }

    #region Public Methods
    public async Task<BaseResponseModel<GetAllTexturesResponse>> GetAllTextures()
    {
        try
        {
            var textures = await GetAllTexturesForResponse();

            return new BaseResponseModel<GetAllTexturesResponse>
            {
                Code = 200,
                Message = "Textures retrieved successfully",
                Data = new GetAllTexturesResponse()
                {
                    ImageUrls = textures
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllTexturesResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetAvailableTexturesResponse>> GetAvailableTextures()
    {
        try
        {
            var textures = await GetAvailableTexturesForResponse();

            return new BaseResponseModel<GetAvailableTexturesResponse>
            {
                Code = 200,
                Message = "Available textures retrieved successfully",
                Data = new GetAvailableTexturesResponse()
                {
                    ImageUrls = textures
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAvailableTexturesResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetTextureByIdResponse>> GetTextureById(GetTextureByIdRequest request)
    {
        try
        {
            var texture = await GetTextureFromId(request);

            return new BaseResponseModel<GetTextureByIdResponse>
            {
                Code = 200,
                Message = "Texture retrieved successfully",
                Data = new GetTextureByIdResponse()
                {
                    ImageUrl = texture.ImageUrl
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetTextureByIdResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<GetTexturesByUserIdResponse>> GetTexturesByUserId(GetTexturesByUserIdRequest request)
    {
        try
        {
            var textures = await GetTexturesFromUserId(request.UserId);

            return new BaseResponseModel<GetTexturesByUserIdResponse>
            {
                Code = 200,
                Message = "User textures retrieved successfully",
                Data = new GetTexturesByUserIdResponse()
                {
                    ImageUrls = textures
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetTexturesByUserIdResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<AddTextureResponse>> AddTexture(AddTextureRequest request)
    {
        try
        {
            var texture = await GetTextureFromAddTextureRequest(request);

            await _textureRepository.AddAsync(texture);

            return new BaseResponseModel<AddTextureResponse>
            {
                Code = 200,
                Message = "Texture added successfully",
                Data = new AddTextureResponse
                {
                    Id = texture.Id,
                    ImageUrl = texture.ImageUrl
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<AddTextureResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = null
            };
        }
    }

    public async Task<BaseResponseModel<DeleteTextureResponse>> DeleteTexture(DeleteTextureRequest request)
    {
        try
        {
            var texture = await GetTextureFromDeleteTextureRequest(request);

            await _textureRepository.UpdateAsync(texture);

            return new BaseResponseModel<DeleteTextureResponse>
            {
                Code = 200,
                Message = "Texture deleted successfully",
                Data = new DeleteTextureResponse
                {
                    Success = true
                }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<DeleteTextureResponse>
            {
                Code = 500,
                Message = ex.Message,
                Data = new DeleteTextureResponse
                {
                    Success = false
                }
            };
        }
    }
    #endregion

    #region Private Methods
    private async Task<IEnumerable<string>> GetAllTexturesForResponse()
    {
        var textures = await _textureRepository.GetAllAsync();
        if (textures == null || !textures.Any())
        {
            throw new InvalidOperationException("No available textures found");
        }
        return textures.Select(t => t.ImageUrl);
    }

    private async Task<IEnumerable<string>> GetAvailableTexturesForResponse()
    {
        var textures = await _textureRepository.GetAvailableTexturesAsync();
        if (textures == null || !textures.Any())
        {
            throw new InvalidOperationException("No available textures found");
        }
        return textures.Select(t => t.ImageUrl);
    }

    private async Task<Texture> GetTextureFromId(GetTextureByIdRequest request)
    {
        var texture = await _textureRepository.FindAsync(request.Id);
        if (texture == null)
        {
            throw new InvalidOperationException($"Texture with ID {request.Id} not found");
        }
        return texture;
    }

    private async Task<IEnumerable<string>> GetTexturesFromUserId(long userId)
    {
        var textures = await _textureRepository.GetTexturesByUserIdAsync(userId);
        if (textures == null || !textures.Any())
        {
            throw new InvalidOperationException("No textures found for the user");
        }
        return textures.Select(t => t.ImageUrl);
    }

    private async Task<Texture> GetTextureFromAddTextureRequest(AddTextureRequest request)
    {
        if (request.ImageFile != null && request.ImageFile.Length > 0)
        {
            return await GetTextureFromUploadTexture(request);
        }
        else if (!string.IsNullOrEmpty(request.Prompt))
        {
            return await GetTextureFromGeneratedTextureRequest(request);
        }
        else
        {
            throw new InvalidOperationException("No image file or prompt provided");
        }
    }

    private async Task<Texture> GetTextureFromGeneratedTextureRequest(AddTextureRequest request)
    {
        var generateRequest = new GenerateImageRequest
        {
            Prompt = request.Prompt
        };

        var generateResponse = await GenerateAndSaveImage(generateRequest);

        if (generateResponse == null)
        {
            throw new InvalidOperationException($"Failed to generate image");
        }

        return new Texture
        {
            UserId = request.OwnerId,
            ImageUrl = generateResponse.ImageUrl,
            Prompt = request.Prompt,
            IsDeleted = false
        };
    }

    private async Task<Texture> GetTextureFromUploadTexture(AddTextureRequest request)
    {
        DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
        string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
        string fileName = $"texture__uploaded_{formattedDateTime}.jpeg";
        byte[] fileBytes;

        using (var memoryStream = new MemoryStream())
        {
            await request.ImageFile.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        var imageUrl = await UploadToAzureStorage(fileName, fileBytes);
        if (string.IsNullOrEmpty(imageUrl))
        {
            throw new InvalidOperationException("Failed to upload file to Azure Storage.");
        }

        return new Texture
        {
            UserId = request.OwnerId,
            ImageUrl = imageUrl,
            Prompt = request.Prompt,
            IsDeleted = false
        };
    }

    private async Task<Texture> GetTextureFromDeleteTextureRequest(DeleteTextureRequest request)
    {
        var texture = await _textureRepository.FindAsync(request.Id);

        if (texture == null)
        {
            throw new InvalidOperationException($"Texture with ID {request.Id} not found");
        }

        texture.IsDeleted = true;
        texture.UpdatedAt = DateTime.Now;

        return texture;
    }

    private async Task<GenerateImageResponse> GenerateAndSaveImage(GenerateImageRequest request)
    {
        string[] availableModels = {
            "black-forest-labs/FLUX.1-schnell-Free",
            "black-forest-labs/FLUX.1-Dev"
        };

        int modelIndex = 0;
        string currentModel = availableModels[modelIndex];

        var requestBody = new
        {
            prompt = request.Prompt + _promptToImproveImage,
            model = currentModel,
            width = 1024,
            height = 768,
            steps = 4,
            n = 1,
            response_format = "b64_json"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("/v1/images/generations", content);

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseData = JsonSerializer.Deserialize<ImageGenerationResponse>(responseContent);

        if (responseData == null || responseData.Data == null || responseData.Data.Count == 0)
        {
            throw new InvalidOperationException("No image data received from API");
        }

        string formattedDateTime = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");
        string fileName = $"texture_{formattedDateTime}.jpeg";
        byte[] imageBytes = Convert.FromBase64String(responseData.Data[0].B64Json);

        var azureImageUrl = await UploadToAzureStorage(fileName, imageBytes);

        return new GenerateImageResponse
        {
            ImageUrl = azureImageUrl,
            Prompt = request.Prompt,
        };
    }

    private async Task<string> UploadToAzureStorage(string fileName, byte[] fileBytes)
    {
        try
        {
            var blobServiceClient = new BlobServiceClient(_azureConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_azureContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = "image/jpeg"
                });
            }

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Azure Storage upload failed: {ex.Message}");
        }
    }
    #endregion
}
