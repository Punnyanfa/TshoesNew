using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Manufacturer;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var manufacturer = await GetManufacturerEntityById(request);
                var response = await MapToDetailResponse(manufacturer);
                return new BaseResponseModel<GetManufacturerDetailResponse>
                {
                    Code = 200,
                    Message = "Manufacturer retrieved successfully",
                    Data = response
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
                var manufacturer = await GetManufacturerEntityById(new GetManufacturerRequest { Id = request.Id });
                manufacturer.Description = request.Description;
                manufacturer.CommissionRate = request.CommissionRate;
                manufacturer.Status = (ManufacturerStatus)request.Status;
                manufacturer.UpdatedAt = DateTime.UtcNow;

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

        public async Task<BaseResponseModel<bool>> DeleteManufacturer(GetManufacturerRequest request)
        {
            try
            {
                var manufacturer = await GetManufacturerEntityById(request);
                manufacturer.Status = ManufacturerStatus.Inactive;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                await _manufacturerRepository.UpdateAsync(manufacturer);
                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Message = "Manufacturer marked as inactive successfully",
                    Data = true
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
            if (user == null || user.UserRole != UserRole.Manufacturer)
            {
                throw new InvalidOperationException("Only users with Manufacturer role can be added as manufacturers");
            }

            var existingManufacturer = await _manufacturerRepository.GetManufacturerByUserIdAsync(request.UserId);
            if (existingManufacturer != null)
            {
                throw new InvalidOperationException("User already has a Manufacturer");
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
                Services = manufacturer.Services?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CurrentAmount = s.SetServiceAmounts?
                        .FirstOrDefault(a => a.Status == ServiceAmountStatus.Active && (a.EndDate == null || a.EndDate > DateTime.UtcNow))?.Amount
                }).ToList() ?? new List<ServiceDto>(),
                Criterias = manufacturer.ManufacturerCriterias?.Select(mc => new CriteriaDto
                {
                    Id = mc.CriteriaId,
                    Name = mc.Criteria.Name
                }).ToList() ?? new List<CriteriaDto>(),
                CreatedAt = manufacturer.CreatedAt,
                UpdatedAt = manufacturer.UpdatedAt
            };
        }

        #endregion
    }
}