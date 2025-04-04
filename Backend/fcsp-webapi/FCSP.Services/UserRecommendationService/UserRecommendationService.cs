using FCSP.DTOs;
using FCSP.DTOs.UserRecommendation;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.UserRecommendationService
{
    public class UserRecommendationService : IUserRecommendationService
    {
        private readonly IUserRecommendationRepository _userRecommendationRepository;

        public UserRecommendationService(IUserRecommendationRepository userRecommendationRepository)
        {
            _userRecommendationRepository = userRecommendationRepository;
        }

        public async Task<BaseResponseModel<GetUserRecommendationsByUserIdResponse>> GetRecommendationsByUserId(GetUserRecommendationsByUserIdRequest request)
        {
            try
            {
                var recommendations = await _userRecommendationRepository.GetAllAsync();
                var userRecommendations = recommendations.Where(r => r.UserId == request.UserId).ToList();
                
                var recommendationDtos = userRecommendations.Select(r => new UserRecommendationDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    CustomShoeDesignId = r.RecommendDesignId,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList();

                return new BaseResponseModel<GetUserRecommendationsByUserIdResponse>
                {
                    Code = 200,
                    Message = "User recommendations retrieved successfully",
                    Data = new GetUserRecommendationsByUserIdResponse
                    {
                        Recommendations = recommendationDtos
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetUserRecommendationsByUserIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddUserRecommendationResponse>> AddRecommendation(AddUserRecommendationRequest request)
        {
            try
            {
                var recommendation = new UserRecommendation
                {
                    UserId = request.UserId,
                    RecommendDesignId = request.CustomShoeDesignId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedRecommendation = await _userRecommendationRepository.AddAsync(recommendation);

                return new BaseResponseModel<AddUserRecommendationResponse>
                {
                    Code = 200,
                    Message = "Recommendation added successfully",
                    Data = new AddUserRecommendationResponse
                    {
                        Success = true,
                        Id = addedRecommendation.Id
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddUserRecommendationResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new AddUserRecommendationResponse
                    {
                        Success = false,
                        Id = 0
                    }
                };
            }
        }

        public async Task<BaseResponseModel<DeleteUserRecommendationResponse>> DeleteRecommendation(DeleteUserRecommendationRequest request)
        {
            try
            {
                await _userRecommendationRepository.DeleteAsync(request.Id);
                
                return new BaseResponseModel<DeleteUserRecommendationResponse>
                {
                    Code = 200,
                    Message = "Recommendation deleted successfully",
                    Data = new DeleteUserRecommendationResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteUserRecommendationResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new DeleteUserRecommendationResponse
                    {
                        Success = false
                    }
                };
            }
        }
    }
} 