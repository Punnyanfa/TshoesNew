using FCSP.DTOs.Authentication;

namespace FCSP.Services.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    public async Task<UserLoginResponse> GoogleLogin(string token)
    {
        throw new NotImplementedException();
    }
}
