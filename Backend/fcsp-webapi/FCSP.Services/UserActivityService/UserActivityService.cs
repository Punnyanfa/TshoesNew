using FCSP.DTOs;
using FCSP.DTOs.UserActivity;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.UserActivityService
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _userActivityRepository;

        public UserActivityService(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public async Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetAllUserActivities()
        {
            try
            {
                var activities = await _userActivityRepository.GetAllAsync();
                var activityResponses = activities.Select(a => MapToResponse(a)).ToList();
                
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 200,
                    Message = "User activities retrieved successfully",
                    Data = activityResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetUserActivityByIdResponse>> GetUserActivityById(GetUserActivityByIdRequest request)
        {
            try
            {
                var activity = await _userActivityRepository.FindAsync(request.Id);
                if (activity == null)
                {
                    return new BaseResponseModel<GetUserActivityByIdResponse>
                    {
                        Code = 404,
                        Message = $"User activity with ID {request.Id} not found",
                        Data = null
                    };
                }

                return new BaseResponseModel<GetUserActivityByIdResponse>
                {
                    Code = 200,
                    Message = "User activity retrieved successfully",
                    Data = MapToResponse(activity)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetUserActivityByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetActivitiesByUser(GetActivitiesByUserRequest request)
        {
            try
            {
                var activities = await _userActivityRepository.GetActivitiesByUserIdAsync(request.UserId);
                var activityResponses = activities.Select(a => MapToResponse(a)).ToList();
                
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 200,
                    Message = "User activities retrieved successfully",
                    Data = activityResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetActivitiesByDesign(GetActivitiesByDesignRequest request)
        {
            try
            {
                var activities = await _userActivityRepository.GetActivitiesByDesignIdAsync(request.DesignId);
                var activityResponses = activities.Select(a => MapToResponse(a)).ToList();
                
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 200,
                    Message = "User activities retrieved successfully",
                    Data = activityResponses
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetUserActivityByIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddUserActivityResponse>> AddUserActivity(AddUserActivityRequest request)
        {
            try
            {
                var activity = new UserActivity
                {
                    UserId = request.UserId,
                    ViewedDesignId = request.ViewedDesignId,
                    ViewAt = request.ViewAt,
                    ViewDuration = request.ViewDuration,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var addedActivity = await _userActivityRepository.AddAsync(activity);
                
                return new BaseResponseModel<AddUserActivityResponse>
                {
                    Code = 201,
                    Message = "User activity added successfully",
                    Data = new AddUserActivityResponse { UserActivityId = addedActivity.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddUserActivityResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateUserActivityResponse>> UpdateUserActivity(UpdateUserActivityRequest request)
        {
            try
            {
                var activity = await _userActivityRepository.FindAsync(request.Id);
                if (activity == null)
                {
                    return new BaseResponseModel<UpdateUserActivityResponse>
                    {
                        Code = 404,
                        Message = $"User activity with ID {request.Id} not found",
                        Data = null
                    };
                }

                activity.ViewAt = request.ViewAt;
                activity.ViewDuration = request.ViewDuration;
                activity.UpdatedAt = DateTime.UtcNow;

                await _userActivityRepository.UpdateAsync(activity);
                
                return new BaseResponseModel<UpdateUserActivityResponse>
                {
                    Code = 200,
                    Message = "User activity updated successfully",
                    Data = new UpdateUserActivityResponse { UserActivityId = activity.Id }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateUserActivityResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteUserActivityResponse>> DeleteUserActivity(DeleteUserActivityRequest request)
        {
            try
            {
                var activity = await _userActivityRepository.FindAsync(request.Id);
                if (activity == null)
                {
                    return new BaseResponseModel<DeleteUserActivityResponse>
                    {
                        Code = 404,
                        Message = $"User activity with ID {request.Id} not found",
                        Data = null
                    };
                }

                await _userActivityRepository.DeleteAsync(request.Id);
                
                return new BaseResponseModel<DeleteUserActivityResponse>
                {
                    Code = 200,
                    Message = "User activity deleted successfully",
                    Data = new DeleteUserActivityResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteUserActivityResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private GetUserActivityByIdResponse MapToResponse(UserActivity activity)
        {
            return new GetUserActivityByIdResponse
            {
                Id = activity.Id,
                UserId = activity.UserId,
                ViewedDesignId = activity.ViewedDesignId,
                ViewAt = activity.ViewAt,
                ViewDuration = activity.ViewDuration,
                CreatedAt = activity.CreatedAt
            };
        }
    }
} 