using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.SizeService
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<Size> GetSizeByIdAsync(long id)
        {
            return await _sizeRepository.FindAsync(id);
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await _sizeRepository.GetAllAsync();
        }

        public async Task<Size> CreateSizeAsync(Size size)
        {
            return await _sizeRepository.AddAsync(size);
        }

        public async Task UpdateSizeAsync(Size size)
        {
            await _sizeRepository.UpdateAsync(size);
        }

        public async Task DeleteSizeAsync(long id)
        {
            await _sizeRepository.DeleteAsync(id);
        }
    }
} 