using FCSP.DTOs.Authentication;
using FCSP.DTOs;

namespace FCSP.Services.Auth;

public interface IAuthService
{
    public Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request);

    public Task<BaseResponseModel<UserRegisterResponse>> Register(UserRegisterRequest request);
}
