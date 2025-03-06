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

        public async Task<IEnumerable<GetUserActivityByIdResponse>> GetAllUserActivities()
        {
            var activities = await _userActivityRepository.GetAllAsync();
            return activities.Select(a => MapToResponse(a));
        }

        public async Task<GetUserActivityByIdResponse> GetUserActivityById(GetUserActivityByIdRequest request)
        {
            var activity = await _userActivityRepository.FindAsync(request.Id);
            if (activity == null)
            {
                throw new InvalidOperationException($"User activity with ID {request.Id} not found");
            }

            return MapToResponse(activity);
        }

        public async Task<IEnumerable<GetUserActivityByIdResponse>> GetActivitiesByUser(GetActivitiesByUserRequest request)
        {
            var activities = await _userActivityRepository.GetActivitiesByUserIdAsync(request.UserId);
            return activities.Select(a => MapToResponse(a));
        }

        public async Task<IEnumerable<GetUserActivityByIdResponse>> GetActivitiesByDesign(GetActivitiesByDesignRequest request)
        {
            var activities = await _userActivityRepository.GetActivitiesByDesignIdAsync(request.DesignId);
            return activities.Select(a => MapToResponse(a));
        }

        public async Task<AddUserActivityResponse> AddUserActivity(AddUserActivityRequest request)
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
            return new AddUserActivityResponse { UserActivityId = addedActivity.Id };
        }

        public async Task<UpdateUserActivityResponse> UpdateUserActivity(UpdateUserActivityRequest request)
        {
            var activity = await _userActivityRepository.FindAsync(request.Id);
            if (activity == null)
            {
                throw new InvalidOperationException($"User activity with ID {request.Id} not found");
            }

            activity.ViewAt = request.ViewAt;
            activity.ViewDuration = request.ViewDuration;
            activity.UpdatedAt = DateTime.UtcNow;

            await _userActivityRepository.UpdateAsync(activity);
            return new UpdateUserActivityResponse { UserActivityId = activity.Id };
        }

        public async Task<DeleteUserActivityResponse> DeleteUserActivity(DeleteUserActivityRequest request)
        {
            var activity = await _userActivityRepository.FindAsync(request.Id);
            if (activity == null)
            {
                throw new InvalidOperationException($"User activity with ID {request.Id} not found");
            }

            await _userActivityRepository.DeleteAsync(request.Id);
            return new DeleteUserActivityResponse { Success = true };
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