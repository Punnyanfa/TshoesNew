using FCSP.DTOs.Texture;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;

namespace FCSP.Services.TextureService
{
    public class TextureService : ITextureService
    {
        private readonly ITextureRepository _textureRepository;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string? _imageSavePath;

        public TextureService(
            ITextureRepository textureRepository,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        { 
            _textureRepository = textureRepository;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("TextureService");
            _imageSavePath = _configuration["ImageStorage:LocalPath"];
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetAllTextures()
        {
            var textures = await _textureRepository.GetAllAsync();
            return textures.Select(t => MapToDetailedResponse(t));
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetAvailableTextures()
        {
            var textures = await _textureRepository.GetAvailableTexturesAsync();
            return textures.Select(t => MapToDetailedResponse(t));
        }

        public async Task<GetTextureByIdResponse> GetTextureById(GetTextureByIdRequest request)
        {
            var texture = await _textureRepository.FindAsync(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException($"Texture with ID {request.Id} not found");
            }

            return MapToDetailedResponse(texture);
        }

        public async Task<IEnumerable<GetTextureByIdResponse>> GetTexturesByUser(GetTexturesByUserRequest request)
        {
            var textures = await _textureRepository.GetTexturesByUserIdAsync(request.UserId);
            return textures.Select(t => MapToDetailedResponse(t));
        }

        public async Task<AddTextureResponse> AddTexture(AddTextureRequest request)
        {
            var texture = new Texture
            {
                UserId = request.OwnerId,
                ImageUrl = request.ImageUrl,
                Prompt = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedTexture = await _textureRepository.AddAsync(texture);
            return new AddTextureResponse 
            { 
                Id = addedTexture.Id,
                Name = request.Name,
                ImageUrl = addedTexture.ImageUrl ?? string.Empty
            };
        }

        public async Task<UpdateTextureResponse> UpdateTexture(UpdateTextureRequest request)
        {
            var texture = await _textureRepository.FindAsync(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException($"Texture with ID {request.Id} not found");
            }
            texture.ImageUrl = request.ImageUrl;
            texture.Prompt = request.Description;
            texture.UpdatedAt = DateTime.UtcNow;

            await _textureRepository.UpdateAsync(texture);
            return new UpdateTextureResponse 
            { 
                Id = texture.Id,
                Name = request.Name,
                ImageUrl = texture.ImageUrl ?? string.Empty
            };
        }

        public async Task<DeleteTextureResponse> DeleteTexture(DeleteTextureRequest request)
        {
            var texture = await _textureRepository.FindAsync(request.Id);
            if (texture == null)
            {
                throw new InvalidOperationException($"Texture with ID {request.Id} not found");
            }

            await _textureRepository.DeleteAsync(request.Id);
            return new DeleteTextureResponse { Success = true };
        }

        public async Task<GenerateImageResponse> GenerateAndSaveImage(GenerateImageRequest request)
        {
            // Define available models for fallback
            string[] availableModels = new string[] {
             
                "black-forest-labs/FLUX.1-schnell-Free"        };
            
            // Use preferred model from request if provided, otherwise use default
            string currentModel = !string.IsNullOrEmpty(request.PreferredModel) 
                ? request.PreferredModel 
                : availableModels[0];
                
            int modelIndex = Array.IndexOf(availableModels, currentModel);
            if (modelIndex < 0) modelIndex = 0; // Use default model if preferred model not in available list
            
            bool retryWithDifferentModel = true;
            
            while (retryWithDifferentModel)
            {
                try
                {
                    var requestBody = new
                    {
                        prompt = request.Prompt + " ,max resolution, no blur",
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
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        // Try next model if available
                        modelIndex++;
                        if (modelIndex < availableModels.Length)
                        {
                            currentModel = availableModels[modelIndex];
                            continue;
                        }
                        else
                        {
                            // If all models failed, throw exception
                            throw new HttpRequestException($"Failed to generate image with all available models. Last error: {response.StatusCode}");
                        }
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    var responseData = JsonSerializer.Deserialize<ImageGenerationResponse>(responseContent);
                    
                    if (responseData == null || responseData.Data == null || responseData.Data.Count == 0)
                    {
                        // Try next model if available
                        modelIndex++;
                        if (modelIndex < availableModels.Length)
                        {
                            currentModel = availableModels[modelIndex];
                            continue;
                        }
                        else
                        {
                            throw new InvalidOperationException("No image data received from API after trying all available models");
                        }
                    }

                    // Convert UTC to GMT+7 (UTC+7)
                    DateTime gmtPlus7Time = DateTime.UtcNow.AddHours(7);
                    // Format as day/month/years - hour:minute but make it filename-friendly
                    string formattedDateTime = gmtPlus7Time.ToString("dd-MM-yyyy_HH-mm");
                    string fileName = $"texture_{formattedDateTime}.png";
                    string filePath = Path.Combine(_imageSavePath ?? "images", fileName);

                    Directory.CreateDirectory(_imageSavePath ?? "images");

                    byte[] imageBytes = Convert.FromBase64String(responseData.Data[0].B64Json);
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    // No more retries needed if we reached here
                    retryWithDifferentModel = false;
                    
                    return new GenerateImageResponse
                    {
                        Success = true,
                        ImagePath = filePath,
                        FileName = fileName,
                        ImageUrl = $"/images/{fileName}",
                        Prompt = request.Prompt,
                        ModelUsed = currentModel // Add the model used to the response
                    };
                }
                catch (HttpRequestException ex)
                {
                    // Try next model if available
                    modelIndex++;
                    if (modelIndex < availableModels.Length)
                    {
                        currentModel = availableModels[modelIndex];
                        continue;
                    }
                    
                    // If all models failed, return error
                    return new GenerateImageResponse
                    {
                        Success = false,
                        ErrorMessage = $"API request failed with all available models. Last error: {ex.Message}",
                        ModelUsed = currentModel
                    };
                }
                catch (JsonException ex)
                {
                    // Try next model if available
                    modelIndex++;
                    if (modelIndex < availableModels.Length)
                    {
                        currentModel = availableModels[modelIndex];
                        continue;
                    }
                    
                    // If all models failed, return error
                    return new GenerateImageResponse
                    {
                        Success = false,
                        ErrorMessage = $"Failed to parse API response with all available models. Last error: {ex.Message}",
                        ModelUsed = currentModel
                    };
                }
                catch (Exception ex)
                {
                    // For other exceptions, don't retry but return with error
                    return new GenerateImageResponse
                    {
                        Success = false,
                        ErrorMessage = $"An unexpected error occurred: {ex.Message}",
                        ModelUsed = currentModel
                    };
                }
            }
            
            // This should not be reached, but is here for code completeness
            return new GenerateImageResponse
            {
                Success = false,
                ErrorMessage = "An unexpected error occurred in the image generation process",
                ModelUsed = currentModel
            };
        }

        private GetTextureByIdResponse MapToDetailedResponse(Texture texture)
        {
            return new GetTextureByIdResponse
            {
                Id = texture.Id,
                Name = texture.Prompt ?? string.Empty,
                Description = texture.Prompt ?? string.Empty,
                ImageUrl = texture.ImageUrl ?? string.Empty,
                IsAvailable = true,
                OwnerId = texture.UserId,
                CreatedAt = texture.CreatedAt,
                UpdatedAt = texture.UpdatedAt
            };
        }
    }
}
