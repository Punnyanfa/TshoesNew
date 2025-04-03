using FCSP.DTOs.Size;
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

        public async Task<SizeResponse> GetSizeByIdAsync(GetSizeByIdRequest request)
        {
            var size = await _sizeRepository.FindAsync(request.Id);
            if (size == null)
            {
                return new SizeResponse
                {
                    Code = 404,
                    Message = "Size not found"
                };
            }

            return new SizeResponse
            {
                Code = 200,
                Message = "Size retrieved successfully",
                Data = MapToDto(size)
            };
        }

        public async Task<SizeListResponse> GetAllSizesAsync()
        {
            var sizes = await _sizeRepository.GetAllAsync();
            return new SizeListResponse
            {
                Code = 200,
                Message = "Sizes retrieved successfully",
                Data = sizes.Select(s => MapToDto(s)).ToList()
            };
        }

        public async Task<SizeResponse> CreateSizeAsync(AddSizeRequest request)
        {
            var newSize = new Size
            {
                SizeValue = request.SizeValue,
                IsDeleted = false
            };

            var createdSize = await _sizeRepository.AddAsync(newSize);
            return new SizeResponse
            {
                Code = 201,
                Message = "Size created successfully",
                Data = MapToDto(createdSize)
            };
        }

        public async Task<SizeResponse> UpdateSizeAsync(UpdateSizeRequest request)
        {
            var size = await _sizeRepository.FindAsync(request.Id);
            if (size == null)
            {
                return new SizeResponse
                {
                    Code = 404,
                    Message = "Size not found"
                };
            }

            size.SizeValue = request.SizeValue;
            size.IsDeleted = request.IsDeleted;

            await _sizeRepository.UpdateAsync(size);
            return new SizeResponse
            {
                Code = 200,
                Message = "Size updated successfully",
                Data = MapToDto(size)
            };
        }

        public async Task<SizeResponse> DeleteSizeAsync(DeleteSizeRequest request)
        {
            var size = await _sizeRepository.FindAsync(request.Id);
            if (size == null)
            {
                return new SizeResponse
                {
                    Code = 404,
                    Message = "Size not found"
                };
            }

            await _sizeRepository.DeleteAsync(request.Id);
            return new SizeResponse
            {
                Code = 200,
                Message = "Size deleted successfully"
            };
        }

        private SizeDto MapToDto(Size size)
        {
            return new SizeDto
            {
                Id = size.Id,
                SizeValue = size.SizeValue,
                IsDeleted = size.IsDeleted
            };
        }
    }
} 