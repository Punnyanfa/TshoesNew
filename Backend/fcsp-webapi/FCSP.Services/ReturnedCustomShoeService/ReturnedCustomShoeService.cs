using FCSP.DTOs;
using FCSP.DTOs.ReturnedCustomShoe;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.ReturnedCustomShoeService
{
    public class ReturnedCustomShoeService : IReturnedCustomShoeService
    {
        private readonly IReturnedCustomShoeRepository _returnedCustomShoeRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;

        public ReturnedCustomShoeService(
            IReturnedCustomShoeRepository returnedCustomShoeRepository,
            ICustomShoeDesignRepository customShoeDesignRepository)
        {
            _returnedCustomShoeRepository = returnedCustomShoeRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
        }

        public async Task<BaseResponseModel<AddReturnedCustomShoeResponse>> AddReturnedCustomShoe(AddReturnedCustomShoeRequest request)
        {
            try
            {
                var design = await _customShoeDesignRepository.FindAsync(request.CustomShoeDesignId);
                if (design == null)
                {
                    throw new InvalidOperationException($"Custom shoe design with ID {request.CustomShoeDesignId} not found");
                }

                var returnedShoe = new ReturnedCustomShoe
                {
                    CustomShoeDesignId = request.CustomShoeDesignId,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _returnedCustomShoeRepository.AddAsync(returnedShoe);

                return new BaseResponseModel<AddReturnedCustomShoeResponse>
                {
                    Code = 200,
                    Message = "Returned custom shoe added successfully",
                    Data = new AddReturnedCustomShoeResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddReturnedCustomShoeResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new AddReturnedCustomShoeResponse { Success = false }
                };
            }
        }

        public async Task<BaseResponseModel<GetReturnedCustomShoesResponse>> GetAllReturnedCustomShoes()
        {
            try
            {
                var returnedShoes = await _returnedCustomShoeRepository.GetAllAsync();
                
                return new BaseResponseModel<GetReturnedCustomShoesResponse>
                {
                    Code = 200,
                    Message = "Returned custom shoes retrieved successfully",
                    Data = new GetReturnedCustomShoesResponse
                    {
                        ReturnedShoes = returnedShoes.Select(MapToReturnedCustomShoeDto)
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetReturnedCustomShoesResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetReturnedCustomShoeByIdResponse>> GetReturnedCustomShoeById(GetReturnedCustomShoeByIdRequest request)
        {
            try
            {
                var returnedShoe = await _returnedCustomShoeRepository.FindAsync(request.Id);
                if (returnedShoe == null)
                {
                    throw new InvalidOperationException($"Returned custom shoe with ID {request.Id} not found");
                }

                return new BaseResponseModel<GetReturnedCustomShoeByIdResponse>
                {
                    Code = 200,
                    Message = "Returned custom shoe retrieved successfully",
                    Data = new GetReturnedCustomShoeByIdResponse
                    {
                        Id = returnedShoe.Id,
                        CustomShoeDesignId = returnedShoe.CustomShoeDesignId,
                        Price = returnedShoe.Price,
                        Quantity = returnedShoe.Quantity,
                        CreatedAt = returnedShoe.CreatedAt,
                        UpdatedAt = returnedShoe.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetReturnedCustomShoeByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetReturnedCustomShoesResponse>> GetReturnedCustomShoesByDesignId(long customShoeDesignId)
        {
            try
            {
                var returnedShoes = await _returnedCustomShoeRepository.GetByCustomShoeDesignIdAsync(customShoeDesignId);
                
                return new BaseResponseModel<GetReturnedCustomShoesResponse>
                {
                    Code = 200,
                    Message = "Returned custom shoes retrieved successfully",
                    Data = new GetReturnedCustomShoesResponse
                    {
                        ReturnedShoes = returnedShoes.Select(MapToReturnedCustomShoeDto)
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetReturnedCustomShoesResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private ReturnedCustomShoeDto MapToReturnedCustomShoeDto(ReturnedCustomShoe returnedShoe)
        {
            return new ReturnedCustomShoeDto
            {
                Id = returnedShoe.Id,
                CustomShoeDesignId = returnedShoe.CustomShoeDesignId,
                Price = returnedShoe.Price,
                Quantity = returnedShoe.Quantity,
                CreatedAt = returnedShoe.CreatedAt
            };
        }
    }
} 