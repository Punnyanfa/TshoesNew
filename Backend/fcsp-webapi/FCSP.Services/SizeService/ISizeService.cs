using FCSP.DTOs.Size;
using FCSP.Models.Entities;

namespace FCSP.Services.SizeService
{
    public interface ISizeService
    {
        // Define methods for Size service
        Task<SizeResponse> GetSizeByIdAsync(GetSizeByIdRequest request);
        Task<SizeListResponse> GetAllSizesAsync();
        Task<SizeResponse> CreateSizeAsync(AddSizeRequest request);
        Task<SizeResponse> UpdateSizeAsync(UpdateSizeRequest request);
        Task<SizeResponse> DeleteSizeAsync(DeleteSizeRequest request);
    }
} 