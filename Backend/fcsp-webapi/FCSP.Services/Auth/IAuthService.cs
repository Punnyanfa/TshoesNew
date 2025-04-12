using FCSP.DTOs;
using FCSP.DTOs.Authentication;

namespace FCSP.Services.Auth;

public interface IAuthService
{
    string HashPassword(string password);
    Task<BaseResponseModel<GetAllUsersResponse>> GetAllUsers();
    Task<BaseResponseModel<GetUserByIdResponse>> GetUserById(GetUserByIdRequest request);
    Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request);
    Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request);
    Task<BaseResponseModel<UpdateUserPasswordResponse>> UpdateUserPassword(UpdateUserPasswordRequest request);
    Task<BaseResponseModel<UpdateUserInformationResponse>> UpdateUserInformation(UpdateUserInformationRequest request);
    Task<BaseResponseModel<UserDeleteResponse>> DeleteUser(UserDeleteRequest request);
    Task<BaseResponseModel<UpdateUserRoleResponse>> UpdateUserRole(UpdateUserRoleRequest request); // Phương thức mới
}