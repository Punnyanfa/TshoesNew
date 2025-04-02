using FCSP.Models.Entities;

namespace FCSP.Services.SizeService
{
    public interface ISizeService
    {
        // Define methods for Size service
        Task<Size> GetSizeByIdAsync(long id);
        Task<IEnumerable<Size>> GetAllSizesAsync();
        Task<Size> CreateSizeAsync(Size size);
        Task UpdateSizeAsync(Size size);
        Task DeleteSizeAsync(long id);
    }
} 