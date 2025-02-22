using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using FCSP.Repositories;

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
            ShippingInfo shippingInfo = GetEntityFromGetByIdRequest(request);
            return new GetShippingInfoByIdResponse
            {
                UserId = shippingInfo.UserId,
                StreetAddress = shippingInfo.StreetAddress,
                Ward = shippingInfo.Ward,
                District = shippingInfo.District,
                City = shippingInfo.City,
                Country = shippingInfo.Country,
                PhoneNumber = shippingInfo.PhoneNumber,
            };
        }

        public async Task<GetShippingInfoByIdResponse> AddShippingInfo(AddShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = GetEntityFromAddRequest(request);
            await _shippingInfoRepository.AddAsync(shippingInfo);
            return new GetShippingInfoByIdResponse();
        }

        public async Task<GetShippingInfoByIdResponse> UpdateShippingInfo(UpdateShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = GetEntityFromUpdateRequest(request);
            await _shippingInfoRepository.UpdateAsync(shippingInfo);
            return new GetShippingInfoByIdResponse();
        }

        public async Task<GetShippingInfoByIdResponse> DeleteShippingInfo(DeleteShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = GetEntityFromDeleteRequest(request);
            await _shippingInfoRepository.DeleteAsync(shippingInfo.Id);
            return new GetShippingInfoByIdResponse();
        }

        private ShippingInfo GetEntityFromGetByIdRequest(GetShippingInfoByIdRequest request)
        {
            ShippingInfo shippingInfo = _shippingInfoRepository.Find(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException();
            }
            return shippingInfo;
        }

        private ShippingInfo GetEntityFromAddRequest(AddShippingInfoRequest request)
        {
            return new ShippingInfo
            {
                UserId = request.UserId,
                StreetAddress = request.StreetAddress,
                Ward = request.Ward,
                District = request.District,
                City = request.City,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
            };
        }

        private ShippingInfo GetEntityFromUpdateRequest(UpdateShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = _shippingInfoRepository.Find(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException();
            }
            shippingInfo.StreetAddress = request.StreetAddress ?? shippingInfo.StreetAddress;
            shippingInfo.Ward = request.Ward ?? shippingInfo.Ward;
            shippingInfo.District = request.District ?? shippingInfo.District;
            shippingInfo.City = request.City ?? shippingInfo.City;
            shippingInfo.Country = request.Country ?? shippingInfo.Country;
            shippingInfo.PhoneNumber = request.PhoneNumber ?? shippingInfo.PhoneNumber;
            shippingInfo.UpdatedAt = DateTime.Now;
            return shippingInfo;
        }

        private ShippingInfo GetEntityFromDeleteRequest(DeleteShippingInfoRequest request)
        {
            ShippingInfo shippingInfo = _shippingInfoRepository.Find(request.Id);
            if (shippingInfo == null)
            {
                throw new InvalidOperationException();
            }
            return shippingInfo;

        }
    }
}
