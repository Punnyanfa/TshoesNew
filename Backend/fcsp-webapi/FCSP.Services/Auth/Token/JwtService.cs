using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using FCSP.Common.Configurations;

namespace FCSP.Services.Authentication.Token;

public class JwtService : ITokenService
{
    #region Fields
    private readonly JwtConfigurations _config;
    #endregion

    #region Constructors
    public JwtService(JwtConfigurations config)
    {
        this._config = config;
    }
    #endregion

    #region Public methods
    public string GetToken()
    {
        return GetJwt();
    }
    #endregion

    #region Private methods
    private string GetJwt()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddMinutes(_config.ExpiryMinutes);

        var token = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            null,
            null,
            expiry,
            creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    #endregion
}
