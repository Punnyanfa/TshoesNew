using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using FCSP.Common;
using FCSP.Common.Options;

namespace FCSP.Services.Authentication;

public class JwtService : ITokenService
{
    #region Fields
    private readonly JwtOptions jwtOptions;
    #endregion

    #region Constructors
    public JwtService(JwtOptions jwtOptions)
    {
        this.jwtOptions = jwtOptions;
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddMinutes(jwtOptions.ExpiryMinutes);

        var token = new JwtSecurityToken(
            jwtOptions.Issuer,
            jwtOptions.Audience,
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
