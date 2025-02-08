using FCSP.DTOs.Authentication;

namespace FCSP.Services.Authentication;

public interface IAuthService
{
    public Task<UserLoginResponse> Login(UserLoginRequest request);

    public Task<UserRegisterResponse> Register(UserRegisterRequest request);
}
