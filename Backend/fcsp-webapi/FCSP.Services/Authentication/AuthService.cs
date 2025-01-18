using FCSP.DTOs.Authentication;

namespace FCSP.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly ITokenService tokenService;

    public AuthService(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    public UserLoginResponse Login(UserLoginRequest request)
    {
        var response = new UserLoginResponse();
        response.Token = tokenService.GetToken();

        return response;
    }
}
