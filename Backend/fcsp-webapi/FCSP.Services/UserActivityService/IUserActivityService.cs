using FCSP.DTOs.UserActivity;

namespace FCSP.Services.UserActivityService
{
    public interface IUserActivityService
    {
        Task<IEnumerable<GetUserActivityByIdResponse>> GetAllUserActivities();
        Task<GetUserActivityByIdResponse> GetUserActivityById(GetUserActivityByIdRequest request);
        Task<IEnumerable<GetUserActivityByIdResponse>> GetActivitiesByUser(GetActivitiesByUserRequest request);
        Task<IEnumerable<GetUserActivityByIdResponse>> GetActivitiesByDesign(GetActivitiesByDesignRequest request);
        Task<AddUserActivityResponse> AddUserActivity(AddUserActivityRequest request);
        Task<UpdateUserActivityResponse> UpdateUserActivity(UpdateUserActivityRequest request);
        Task<DeleteUserActivityResponse> DeleteUserActivity(DeleteUserActivityRequest request);
    }
} 