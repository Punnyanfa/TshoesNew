using FCSP.Services.Auth;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using Microsoft.AspNet.Identity;

namespace FCSP.WebAPI.Configuration;

internal static class ServiceConfig
{
    public static void Configure(IServiceCollection services)
    {
        RegisterServices(services);
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<ITokenService, JwtService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
    }
}
