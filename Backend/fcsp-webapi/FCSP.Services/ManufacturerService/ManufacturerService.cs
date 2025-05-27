using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace FCSP.Services.ManufacturerService
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository, IUserRepository userRepository)
        {
            _manufacturerRepository = manufacturerRepository;
            _userRepository = userRepository;
        }

        #region Public Methods

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetAllManufacturers()
        {
            try
            {
                var manufacturers = await GetAllManufacturersWithDetails();
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Manufacturers retrieved successfully",
                    Data = manufacturers
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetManufacturerDetailResponse>> GetManufacturerById(GetManufacturerRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                   
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 400, Message = "Manufacturer ID must be greater than 0" };
                }

               
                var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
                if (manufacturer == null)
                {
                    
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 404, Message = "Manufacturer not found" };
                }

                if (manufacturer.Status == ManufacturerStatus.Suspended)
                {
                    return new BaseResponseModel<GetManufacturerDetailResponse> { Code = 404, Message = "Manufacturer is suspended" };
                }

                return new BaseResponseModel<GetManufacturerDetailResponse>
                {
                    Code = 200,
                    Message = "Manufacturer retrieved successfully",
                    Data = await MapToDetailResponse(manufacturer)
               
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetManufacturerDetailResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetManufacturersByUserId(long userId)
        {
            try
            {
                var manufacturers = await GetManufacturersByUserIdAsync(userId);
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Manufacturers retrieved successfully for user",
                    Data = manufacturers
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<GetManufacturerDetailResponse>>> GetActiveManufacturers()
        {
            try
            {
                var manufacturers = await GetActiveManufacturersAsync();
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 200,
                    Message = "Active manufacturers retrieved successfully",
                    Data = manufacturers
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetManufacturerDetailResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddManufacturerResponse>> AddManufacturer(AddManufacturerRequest request)
        {
            try
            {                           
                var manufacturer = await CreateManufacturerFromRequest(request);
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (manufacturer.UserId == user.Id)
                {
                    user.UserRole = UserRole.Manufacturer;
                }
                var addedManufacturer = await _manufacturerRepository.AddAsync(manufacturer);
               
                return new BaseResponseModel<AddManufacturerResponse>
                {
                    Code = 201,
                    Message = "Manufacturer added successfully",
                    Data = new AddManufacturerResponse
                    {
                        Id = addedManufacturer.Id,
                        Description = addedManufacturer.Description,
                        CreatedAt = addedManufacturer.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddManufacturerResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateManufacturerResponse>> UpdateManufacturer(UpdateManufacturerRequest request)
        {
            try
            { 
                var manufacturer = await GetManufacturerFromUpdateRequest(request);
                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<UpdateManufacturerResponse>
                {
                    Code = 200,
                    Message = "Manufacturer updated successfully",
                    Data = new UpdateManufacturerResponse
                    {
                        Id = manufacturer.Id,
                        Description = manufacturer.Description,
                        Status = manufacturer.Status.ToString(),
                        UpdatedAt = manufacturer.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateManufacturerResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<BaseResponseModel<UpdateManufacturerStatusResponse>> UpdateManufacturerStatus(UpdateManufacturerStatusRequest request)
        {
            try
            {
                var manufacturer = await GetManufacturer(request.Id);
                manufacturer.Status = (ManufacturerStatus)request.Status;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<UpdateManufacturerStatusResponse>
                {
                    Code = 200,
                    Message = "Manufacturer status updated successfully",
                    Data = new UpdateManufacturerStatusResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateManufacturerStatusResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<bool>> DeleteManufacturer(GetManufacturerRequest request)
        {
            try
            {
              var manufacturer = await DeleteManufacturerFromRequest(request);
                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Message = "Manufacturer marked as inactive successfully",
                    Data = true
                };
            }
            catch (InvalidOperationException ex)
            {
                return new BaseResponseModel<bool>
                {
                    Code = ex.Message.Contains("not found") ? 404 : 400,
                    Message = ex.Message,
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<GetManufacturerDetailResponse>> GetAllManufacturersWithDetails()
        {
            var manufacturers = await _manufacturerRepository.GetAllWithDetailsAsync();
            var result = new List<GetManufacturerDetailResponse>();
            foreach (var manufacturer in manufacturers)
            {
                result.Add(await MapToDetailResponse(manufacturer));
            }
            return result;
        }
        private async Task<Manufacturer> GetManufacturer(long manuId)
        {
            var manufacturer = await _manufacturerRepository.FindAsync(manuId);
            if (manufacturer == null)
                throw new ArgumentException("manufacturer not found");
            return manufacturer;
        }
        private async Task<Manufacturer> GetManufacturerEntityById(GetManufacturerRequest request)
        {
            if (request.Id <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than 0");
            }
            var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
            if (manufacturer == null)
            {
                throw new InvalidOperationException("Manufacturer not found");
            }
            return manufacturer;
        }
        private async Task<Manufacturer> GetManufacturerFromUpdateRequest(UpdateManufacturerRequest request)
        {
            // Validate Id
            if (request.Id <= 0)
            {
                throw new ArgumentException("Manufacturer ID must be greater than 0");
            }
            // Validate Description
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                throw new ArgumentException("Description can not be empty");
            }
            var words = request.Description.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 5)
            {
                throw new ArgumentException("Description must be greater than 5 words");
            }
            if (words.Length > 50)
            {
                throw new ArgumentException("Description must be less than 50 words");
            }

            // Validate CommissionRate
            if (request.CommissionRate <= 0)
            {
                throw new ArgumentException("CommissionRate can not be empty");
            }
            if (request.CommissionRate < 5)
            {
                throw new ArgumentException("commissionRate must be greater than 5");
            }
            if (request.CommissionRate > 50)
            {
                throw new ArgumentException("commissionRate must be less than 50");
            }
            // Validate Status
            if (!Enum.IsDefined(typeof(ManufacturerStatus), request.Status))
            {
                throw new ArgumentException("status is invalid");
            }
            var manufacturer = await GetManufacturerEntityById(new GetManufacturerRequest { Id = request.Id });
            manufacturer.Description = request.Description;
            manufacturer.CommissionRate = request.CommissionRate;
            manufacturer.Status = (ManufacturerStatus)request.Status;
            manufacturer.UpdatedAt = DateTime.UtcNow;           
            return manufacturer;
        }
        private async Task<List<GetManufacturerDetailResponse>> GetManufacturersByUserIdAsync(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0");
            }

            var manufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(userId);
            if (manufacturer == null)
            {
                throw new InvalidOperationException("No manufacturers found for this user");
            }

            return new List<GetManufacturerDetailResponse> { await MapToDetailResponse(manufacturer) };
        }
        private async Task<List<GetManufacturerDetailResponse>> GetActiveManufacturersAsync()
        {
            var manufacturers = await _manufacturerRepository.GetManufacturersByStatusAsync((int)ManufacturerStatus.Active);
            var result = new List<GetManufacturerDetailResponse>();
            foreach (var manufacturer in manufacturers)
            {
                result.Add(await MapToDetailResponse(manufacturer));
            }
            return result;
        }
        private async Task<Manufacturer> CreateManufacturerFromRequest(AddManufacturerRequest request)
        {

            var user = await _userRepository.GetByIdAsync(request.UserId);               
            if (request.UserId <= 0)
            {
                throw new ArgumentException("UserId is required");
            }      
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }          
            var existingManufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(request.UserId);
            if (existingManufacturer != null)
            {
                throw new InvalidOperationException("User already has a Manufacturer");
            }
            // Validate Description
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                throw new ArgumentException("Description can not be empty");
            }
            var words = request.Description.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 5)
            {
                throw new ArgumentException("Description must be greater than 5 words");
            }
            if (words.Length > 50)
            {
                throw new ArgumentException("Description must be less than 50 words");
            }
            // Validate CommissionRate
            if (request.CommissionRate <= 0)
            {
                throw new ArgumentException("CommissionRate can not be empty");
            }
            if (request.CommissionRate < 5)
            {
                throw new ArgumentException("commissionRate must be greater than 5");
            }
            if (request.CommissionRate > 50)
            {
                throw new ArgumentException("commissionRate must be less than 50");
            }
            // Validate Status
            if (!Enum.IsDefined(typeof(ManufacturerStatus), request.Status))
            {
                throw new ArgumentException("status is invalid");
            }
            return new Manufacturer
            {
                UserId = request.UserId,
                Description = request.Description,
                CommissionRate = request.CommissionRate,
                Status = (ManufacturerStatus)request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        private async Task<Manufacturer> DeleteManufacturerFromRequest(GetManufacturerRequest request)
        {
            if (request.Id <= 0)
            {
                throw new InvalidOperationException("Manufacturer ID must be greater than 0");
               
            }
            var manufacturer = await _manufacturerRepository.GetManufacturerWithDetailsAsync(request.Id);
            if (manufacturer == null)
            {
                throw new InvalidOperationException("Manufacturer not found");
            }

            manufacturer.IsDeleted = true;
            manufacturer.Status = ManufacturerStatus.Inactive;
            manufacturer.UpdatedAt = DateTime.UtcNow;
            return manufacturer;
        }
        private async Task<GetManufacturerDetailResponse> MapToDetailResponse(Manufacturer manufacturer)
        {
            var user = await _userRepository.GetUserNameByUserIdAsync(manufacturer.UserId);
            return new GetManufacturerDetailResponse
            {
                Id = manufacturer.Id,
                UserName = user?.Name ?? "N/A",
                Description = manufacturer.Description,
                Status = manufacturer.Status.ToString(),
                CommissionRate = manufacturer.CommissionRate,
                Services = manufacturer.Services?.Where(s => s.IsDeleted != true).Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Component = s.Component,
                    Type = s.Type,
                    CurrentAmount = s.Price
                }).ToList() ?? new List<ServiceDto>(),
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt
            };
        }
        #endregion
    }
}