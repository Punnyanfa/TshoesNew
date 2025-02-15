using FCSP.DTOs.Authentication;

namespace FCSP.Services.Auth;

public interface IGoogleAuthService
{
    public Task<UserLoginResponse> GoogleLogin(string token);
}
