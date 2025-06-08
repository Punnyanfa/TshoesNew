using FCSP.Common.Configurations;
using FCSP.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using FCSP.Common.Utils;

namespace FCSP.Services.Auth.Token;

public class JwtService : ITokenService
{
    #region Fields
    private readonly JwtConfigurations _config;
    #endregion

    #region Constructors
    public JwtService(JwtConfigurations config)
    {
        _config = config;
    }
    #endregion

    #region Public methods
    public string GetToken(User user)
    {
        return GetJwt(user);
    }
    #endregion

    #region Private methods
    private string GetJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTimeUtils.GetCurrentGmtPlus7().AddMinutes(_config.ExpiryMinutes);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        if (user.UserRole == Common.Enums.UserRole.Manufacturer)
        {
            var manufacturerId = user.Manufacturers.FirstOrDefault(u => u.UserId == user.Id)?.Id ?? 0;
            claims.Add(new Claim("ManufacturerId", manufacturerId.ToString()));
        }else if (user.UserRole == Common.Enums.UserRole.Designer)
        {
            var designerId = user.Designers.FirstOrDefault(u => u.UserId == user.Id)?.Id ?? 0;
            claims.Add(new Claim("DesignerId", designerId.ToString()));
        }

        var token = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            null,
            expiry,
            creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    #endregion
}
