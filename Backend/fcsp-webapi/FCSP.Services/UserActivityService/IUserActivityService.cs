using FCSP.DTOs;
using FCSP.DTOs.UserActivity;

namespace FCSP.Services.UserActivityService
{
    public interface IUserActivityService
    {
        Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetAllUserActivities();
        Task<BaseResponseModel<GetUserActivityByIdResponse>> GetUserActivityById(GetUserActivityByIdRequest request);
        Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetActivitiesByUser(GetActivitiesByUserRequest request);
        Task<BaseResponseModel<List<GetUserActivityByIdResponse>>> GetActivitiesByDesign(GetActivitiesByDesignRequest request);
        Task<BaseResponseModel<AddUserActivityResponse>> AddUserActivity(AddUserActivityRequest request);
        Task<BaseResponseModel<UpdateUserActivityResponse>> UpdateUserActivity(UpdateUserActivityRequest request);
        Task<BaseResponseModel<DeleteUserActivityResponse>> DeleteUserActivity(DeleteUserActivityRequest request);
    }
}