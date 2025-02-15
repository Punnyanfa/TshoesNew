using FCSP.DTOs.Authentication;
using FCSP.Services.Auth;

namespace FCSP.Services.Authentication;

public class GoogleAuthService : IGoogleAuthService
{
    public async Task<UserLoginResponse> GoogleLogin(string token)
    {
        throw new NotImplementedException();
    }
}
