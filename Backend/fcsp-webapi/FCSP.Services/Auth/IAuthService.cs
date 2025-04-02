using FCSP.DTOs.Authentication;
using FCSP.DTOs;

namespace FCSP.Services.Auth;

public interface IAuthService
{
    public string HashPassword(string password);

    public Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request);

    public Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request);

    public Task<BaseResponseModel<UpdateUserPasswordResponse>> UpdateUserPassword(UpdateUserPasswordRequest request);

    public Task<BaseResponseModel<UpdateUserInformationResponse>> UpdateUserInformation(UpdateUserInformationRequest request);

    public Task<BaseResponseModel<UserDeleteResponse>> DeleteUser(UserDeleteRequest request);
}
