using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using FCSP.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.ShippingInfoService
{
    public class ShippingInfoService : IShippingInfoService
    {
        private readonly IShippingInfoRepository _shippingInfoRepository;

        public ShippingInfoService(IShippingInfoRepository shippingInfoRepository)
        {
            _shippingInfoRepository = shippingInfoRepository;
        }

        public async Task<IEnumerable<ShippingInfo>> GetAllShippingInfo()
        {
            var response = await _shippingInfoRepository.GetAllAsync();
            return response;
        }

        public async Task<GetShippingInfoByIdResponse> GetShippingInfoById(GetShippingInfoByIdRequest request)
        {
            ShippingInfo shippingInfo = await GetEntityFromGetByIdRequest(request);
            return new GetShippingInfoByIdResponse
            {
                Id = shippingInfo.Id,
                UserId = shippingInfo.UserId,
                ReceiverName = "N/A", // Set appropriate value if available
                PhoneNumber = shippingInfo.PhoneNumber ?? string.Empty,
                Address = shippingInfo.StreetAddress ?? string.Empty,
                City = shippingInfo.City ?? string.Empty,
                District = shippingInfo.District ?? string.Empty,
                Ward = shippingInfo.Ward ?? string.Empty,
                IsDefault = false, // Set appropriate value if available
                CreatedAt = shippingInfo.CreatedAt,
                UpdatedAt = shippingInfo.UpdatedAt
            };
        }

        public async Task<AddShippingInfoResponse> AddShippingInfo(AddShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = GetEntityFromAddRequest(request);
            var addedShippingInfo = await _shippingInfoRepository.AddAsync(shippingInfo);
            return new AddShippingInfoResponse
            {
                Id = addedShippingInfo.Id,
                ReceiverName = "N/A", // Set appropriate value if available
                Address = addedShippingInfo.StreetAddress ?? string.Empty,
                IsDefault = false // Set appropriate value if available
            };
        }

        public async Task<UpdateShippingInfoResponse> UpdateShippingInfo(UpdateShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = await GetEntityFromUpdateRequest(request);
            await _shippingInfoRepository.UpdateAsync(shippingInfo);
            return new UpdateShippingInfoResponse
            {
                Id = shippingInfo.Id,
                ReceiverName = "N/A", // Set appropriate value if available
                Address = shippingInfo.StreetAddress ?? string.Empty,
                IsDefault = false // Set appropriate value if available
            };
        }

        public async Task<DeleteShippingInfoResponse> DeleteShippingInfo(DeleteShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = await GetEntityFromDeleteRequest(request);
            await _shippingInfoRepository.DeleteAsync(shippingInfo.Id);
            return new DeleteShippingInfoResponse { Success = true };
        }

        private async Task<ShippingInfo> GetEntityFromGetByIdRequest(GetShippingInfoByIdRequest request)
        {
            ShippingInfo shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException("ShippingInfo not found");
            }
            return shippingInfo;
        }

        private ShippingInfo GetEntityFromAddRequest(AddShippingInfoRequest request)
        {
            return new ShippingInfo
            {
                UserId = request.UserId,
                StreetAddress = request.Address,
                Ward = request.Ward,
                District = request.District,
                City = request.City,
                Country = "Vietnam", // Set appropriate default or get from request
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<ShippingInfo> GetEntityFromUpdateRequest(UpdateShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException("ShippingInfo not found");
            }
            
            shippingInfo.StreetAddress = request.Address;
            shippingInfo.Ward = request.Ward;
            shippingInfo.District = request.District;
            shippingInfo.City = request.City;
            shippingInfo.PhoneNumber = request.PhoneNumber;
            shippingInfo.UpdatedAt = DateTime.UtcNow;
            
            return shippingInfo;
        }

        private async Task<ShippingInfo> GetEntityFromDeleteRequest(DeleteShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException("ShippingInfo not found");
            }
            return shippingInfo;
        }
    }
}
