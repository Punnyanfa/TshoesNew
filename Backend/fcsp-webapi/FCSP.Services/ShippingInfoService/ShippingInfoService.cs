using FCSP.DTOs;
using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;

namespace FCSP.Services.ShippingInfoService
{
    public class ShippingInfoService : IShippingInfoService
    {
        private readonly IShippingInfoRepository _shippingInfoRepository;
        private readonly IUserRepository _userRepository;

        public ShippingInfoService(IShippingInfoRepository shippingInfoRepository, IUserRepository userRepository)
        {
            _shippingInfoRepository = shippingInfoRepository;
            _userRepository = userRepository;
        }

        #region Public Methods
        public async Task<BaseResponseModel<GetAllShippingInfoResponse>> GetAllShippingInfo()
        {
            try
            {
                var shippingInfos = await GetAllShippingInfos();
                return new BaseResponseModel<GetAllShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Shipping information retrieved successfully",
                    Data = new GetAllShippingInfoResponse
                    {
                        ShippingInfos = shippingInfos
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetAllShippingInfoResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetShippingInfoByIdResponse>> GetShippingInfoById(GetShippingInfoByIdRequest request)
        {
            try
            {
                var shippingInfo = await GetShippingInfoEntityById(request);
                var response = await MapToDetailResponse(shippingInfo);
                return new BaseResponseModel<GetShippingInfoByIdResponse>
                {
                    Code = 200,
                    Message = "Shipping information retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetShippingInfoByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetShippingInfosByUserResponse>> GetShippingInfosByUserId(GetShippingInfosByUserRequest request)
        {
            try
            {
                var shippingInfos = await GetShippingInfosByUser(request);
                return new BaseResponseModel<GetShippingInfosByUserResponse>
                {
                    Code = 200,
                    Message = "User shipping information retrieved successfully",
                    Data = new GetShippingInfosByUserResponse
                    {
                        ShippingInfos = shippingInfos
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetShippingInfosByUserResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddShippingInfoResponse>> AddShippingInfo(AddShippingInfoRequest request)
        {
            try
            {
                if (request.IsDefault)
                {
                    await SetOtherAddressesToNonDefault(request.UserId);
                }

                var shippingInfo = GetShippingInfoFromAddRequest(request);
                var addedShippingInfo = await _shippingInfoRepository.AddAsync(shippingInfo);

                return new BaseResponseModel<AddShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Shipping information added successfully",
                    Data = new AddShippingInfoResponse
                    {
                        Id = addedShippingInfo.Id,
                        ReceiverName = await GetUserNameById(addedShippingInfo.UserId),
                        Address = addedShippingInfo.StreetAddress ?? string.Empty,
                        IsDefault = addedShippingInfo.IsDefault
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddShippingInfoResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateShippingInfoResponse>> UpdateShippingInfo(UpdateShippingInfoRequest request)
        {
            try
            {
                var shippingInfo = await GetShippingInfoFromUpdateRequest(request);

                if (request.IsDefault && !shippingInfo.IsDefault)
                {
                    await SetOtherAddressesToNonDefault(shippingInfo.UserId);
                }

                shippingInfo.StreetAddress = request.Address;
                shippingInfo.Ward = request.Ward;
                shippingInfo.District = request.District;
                shippingInfo.City = request.City;
                shippingInfo.PhoneNumber = request.PhoneNumber;
                shippingInfo.ContactNumber = request.ReceiverName;
                shippingInfo.IsDefault = request.IsDefault;
                shippingInfo.UpdatedAt = DateTime.UtcNow;

                await _shippingInfoRepository.UpdateAsync(shippingInfo);

                return new BaseResponseModel<UpdateShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Shipping information updated successfully",
                    Data = new UpdateShippingInfoResponse
                    {
                        Id = shippingInfo.Id,
                        ReceiverName = await GetUserNameById(shippingInfo.UserId),
                        Address = shippingInfo.StreetAddress ?? string.Empty,
                        IsDefault = shippingInfo.IsDefault
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateShippingInfoResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteShippingInfoResponse>> DeleteShippingInfo(DeleteShippingInfoRequest request)
        {
            try
            {
                var shippingInfo = await GetShippingInfoFromDeleteRequest(request);
                await _shippingInfoRepository.DeleteAsync(shippingInfo.Id);

                return new BaseResponseModel<DeleteShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Shipping information deleted successfully",
                    Data = new DeleteShippingInfoResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteShippingInfoResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new DeleteShippingInfoResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<SetDefaultShippingInfoResponse>> SetDefaultShippingInfo(SetDefaultShippingInfoRequest request)
        {
            try
            {
                var shippingInfo = await GetShippingInfoForSetDefault(request);

                if (!shippingInfo.IsDefault)
                {
                    await SetOtherAddressesToNonDefault(request.UserId);
                    shippingInfo.IsDefault = true;
                    shippingInfo.UpdatedAt = DateTime.UtcNow;
                    await _shippingInfoRepository.UpdateAsync(shippingInfo);
                }

                return new BaseResponseModel<SetDefaultShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Default shipping information set successfully",
                    Data = new SetDefaultShippingInfoResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<SetDefaultShippingInfoResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new SetDefaultShippingInfoResponse { Success = false }
                };
            }
        }
        public async Task<BaseResponseModel<GetShippingInfoByIdResponse>> GetByOrderId(GetShippingInfoByOrderIdRequest request)
        {
            try
            {
                var shippingInfo = await GetShippingInfoByOrderId(request);
                var response = await MapToDetailResponse(shippingInfo);
                return new BaseResponseModel<GetShippingInfoByIdResponse>
                {
                    Code = 200,
                    Message = "Shipping information retrieved successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetShippingInfoByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        #endregion

        #region Private Methods
        public async Task<string> GetUserNameById(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Name ?? "N/A";
        }
        private async Task<IEnumerable<GetShippingInfoByIdResponse>> GetAllShippingInfos()
        {
            var shippingInfos = await _shippingInfoRepository.GetAllAsync();
            var result = new List<GetShippingInfoByIdResponse>();
            foreach (var shippingInfo in shippingInfos)
            {
                result.Add(await MapToDetailResponse(shippingInfo));
            }
            return result;
        }
        private async Task<ShippingInfo> GetShippingInfoEntityById(GetShippingInfoByIdRequest request)
        {
            var shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information with ID {request.Id} not found");
            }
            return shippingInfo;
        }

        private async Task<IEnumerable<GetShippingInfoByIdResponse>> GetShippingInfosByUser(GetShippingInfosByUserRequest request)
        {
            var shippingInfos = await _shippingInfoRepository.GetByUserIdAsync(request.UserId);
            var result = new List<GetShippingInfoByIdResponse>();
            foreach (var shippingInfo in shippingInfos)
            {
                result.Add(await MapToDetailResponse(shippingInfo));
            }
            return result;
        }
        private ShippingInfo GetShippingInfoFromAddRequest(AddShippingInfoRequest request)
        {
            return new ShippingInfo
            {
                UserId = request.UserId,
                StreetAddress = request.Address,
                Ward = request.Ward,
                District = request.District,
                City = request.City,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
                ContactNumber = "N/A",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDefault = request.IsDefault
            };
        }

        private async Task<ShippingInfo> GetShippingInfoFromUpdateRequest(UpdateShippingInfoRequest request)
        {
            var shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information with ID {request.Id} not found");
            }
            return shippingInfo;
        }

        private async Task<ShippingInfo> GetShippingInfoFromDeleteRequest(DeleteShippingInfoRequest request)
        {
            var shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information with ID {request.Id} not found");
            }
            return shippingInfo;
        }

        private async Task<ShippingInfo> GetShippingInfoForSetDefault(SetDefaultShippingInfoRequest request)
        {
            var shippingInfo = await _shippingInfoRepository.FindAsync(request.Id);
            if (shippingInfo == null || shippingInfo.UserId != request.UserId || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information with ID {request.Id} not found or does not belong to user");
            }
            return shippingInfo;
        }

        private async Task SetOtherAddressesToNonDefault(long userId)
        {
            var addresses = await _shippingInfoRepository.GetByUserIdAsync(userId);
            var defaultAddresses = addresses.Where(a => a.IsDefault);
            foreach (var addr in defaultAddresses)
            {
                addr.IsDefault = false;
                await _shippingInfoRepository.UpdateAsync(addr);
            }
        }
        private async Task<ShippingInfo> GetShippingInfoByOrderId(GetShippingInfoByOrderIdRequest request)
        {
            var shippingInfo = await _shippingInfoRepository.GetByOrderIdAsync(request.OrderId);
            if (shippingInfo == null || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information for Order ID {request.OrderId} not found");
            }
            return shippingInfo;
        }
        private async Task<GetShippingInfoByIdResponse> MapToDetailResponse(ShippingInfo shippingInfo)
        {
            return new GetShippingInfoByIdResponse
            {
                Id = shippingInfo.Id,
                UserId = shippingInfo.UserId,
                ReceiverName = await GetUserNameById(shippingInfo.UserId),
                PhoneNumber = shippingInfo.PhoneNumber ?? string.Empty,
                Address = shippingInfo.StreetAddress ?? string.Empty,
                City = shippingInfo.City ?? string.Empty,
                District = shippingInfo.District ?? string.Empty,
                Ward = shippingInfo.Ward ?? string.Empty,
                IsDefault = shippingInfo.IsDefault,
                CreatedAt = shippingInfo.CreatedAt,
                UpdatedAt = shippingInfo.UpdatedAt
            };
        }

        #endregion
    }
}