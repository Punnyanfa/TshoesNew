using FCSP.DTOs;
using FCSP.DTOs.ShippingInfo;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
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

                var shippingInfo = await GetShippingInfoFromAddRequest(request);
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
            catch(InvalidOperationException ex)
            {
                return new BaseResponseModel<AddShippingInfoResponse>
                {
                    Code = ex.Message.Contains("not found")? 404:400,
                    Message = ex.Message
                    
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
                shippingInfo.UpdatedAt = DateTime.Now;

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
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<DeleteShippingInfoResponse>
                {
                    Code = ex.Message.Contains("not found") ? 404 : 400,
                    Message = ex.Message,
                    Data = ex.Message.Contains("not found") ? null : new DeleteShippingInfoResponse { Success = false }
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
                    shippingInfo.UpdatedAt = DateTime.Now;
                    await _shippingInfoRepository.UpdateAsync(shippingInfo);
                }

                return new BaseResponseModel<SetDefaultShippingInfoResponse>
                {
                    Code = 200,
                    Message = "Default shipping information set successfully",
                    Data = new SetDefaultShippingInfoResponse { Success = true }
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<SetDefaultShippingInfoResponse>
                {
                    Code = ex.Message.Contains("not found") ? 404 : 400,
                    Message = ex.Message,
                    Data = new SetDefaultShippingInfoResponse { Success = false }
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

   
        public async Task<string> GetUserNameById(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Name ?? "N/A";
        }
        #region Private Methods
        private string longString = string.Join(" ", new string[25].Select((_, i) => $"word{i}"));
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
        private async Task< ShippingInfo> GetShippingInfoFromAddRequest(AddShippingInfoRequest request)
        {
            if (request.UserId <= 0)
            {
                throw new InvalidOperationException("User ID must be greater than 0");
            }
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                throw new InvalidOperationException("Phone number is required");
            }
            if (request.PhoneNumber.Length > 25 || request.PhoneNumber.Any(char.IsLetter) || request.PhoneNumber.Any(c => !char.IsDigit(c)))
            {
                throw new InvalidOperationException("Phone number is not correct");
            }
            if (request.PhoneNumber.Length > 25)
            {
                throw new InvalidOperationException("Phone number must not exceed 25 characters");
            }
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                throw new InvalidOperationException("Address is required");
            }
            if(request.Address.Length < 5)
            {
                throw new InvalidOperationException("Address must be at least 5 characters");
            }
            if (request.Address.Length > 25)
            {
                throw new InvalidOperationException("Address must be less than 25 characters");
            }
            if (string.IsNullOrWhiteSpace(request.Ward))
            {
                throw new InvalidOperationException("Ward is required");
            }
            if (request.Ward.Length < 5)
            {
                throw new InvalidOperationException("Ward must be at least 5 characters");
            }
            if (request.Ward.Length > 25)
            {
                throw new InvalidOperationException("Ward must be less than 25 characters");
            }
            if (string.IsNullOrWhiteSpace(request.District))
            {
                throw new InvalidOperationException("District is required");
            }
            if (request.District.Length < 5)
            {
                throw new InvalidOperationException("District must be at least 5 characters");
            }
            if (request.District.Length > 25)
            {
                throw new InvalidOperationException("District must be less than 25 characters");
            }
            if (string.IsNullOrWhiteSpace(request.City))
            {
                throw new InvalidOperationException("City is required");
            }
            if (request.City.Length < 5)
            {
                throw new InvalidOperationException("City must be at least 5 characters");
            }
            if (request.City.Length > 25)
            {
                throw new InvalidOperationException("City must be less than 25 characters");
            }
            if (string.IsNullOrWhiteSpace(request.Country))
            {
                throw new InvalidOperationException("Country is required");
            }
            if (request.Country.Length < 5)
            {
                throw new InvalidOperationException("Country must be at least 5 characters");
            }
            if (request.Country.Length > 25)
            {
                throw new InvalidOperationException("Country must be less than 25 characters");
            }   
            
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null)
            {
                throw new InvalidOperationException($"User with ID {request.UserId} not found");
            }
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
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
            if (request.Id <= 0)
            {
                throw new InvalidOperationException("Shipping info Id is required");
            }

            var shippingInfo = await _shippingInfoRepository.FindAsync(new object[] { request.Id });
            if (shippingInfo == null || shippingInfo.IsDeleted)
            {
                throw new InvalidOperationException($"Shipping information with ID {request.Id} not found");
            }
            return shippingInfo;
        }

        private async Task<ShippingInfo> GetShippingInfoForSetDefault(SetDefaultShippingInfoRequest request)
        {
            if (request.UserId <= 0)
            {
                throw new InvalidOperationException("UserId can not be 0");
            }
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"{request.UserId} not found");
            }
            if (request.Id <= 0)
            {
                throw new InvalidOperationException("Shipping info Id is required");
            }
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