using FCSP.Models.Entities;

namespace FCSP.Services.Auth.Token;

public interface ITokenService
{
    public string GetToken(User user);
}
