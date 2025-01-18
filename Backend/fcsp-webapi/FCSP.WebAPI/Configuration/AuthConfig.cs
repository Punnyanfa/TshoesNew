using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using FCSP.Common.Options;
using FCSP.Common;

namespace FCSP.WebAPI.Configuration;

internal static class AuthConfig
{
    #region Fields
    private static JwtOptions? jwtOptions;
    #endregion

    #region Public methods
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        ConfigureJwtBearer(services, config);
    }
    #endregion

    #region Private methods
    private static void ConfigureJwtBearer(IServiceCollection services, IConfiguration config)
    {
        jwtOptions = config.GetSection(Constants.JwtConfig).Get<JwtOptions>();
        if (jwtOptions == null)
        {
            return;
        }

        services.AddSingleton(jwtOptions);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)
            );

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = key,
            };
        });
        
        services.AddAuthorization();
    }
    #endregion
}
