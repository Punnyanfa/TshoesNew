using FCSP.DTOs.Authentication;

namespace FCSP.Services.Authentication;

public interface IAuthService
{
    public UserLoginResponse Login(UserLoginRequest request);
}
