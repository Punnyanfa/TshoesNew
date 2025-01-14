using FCSP.Services.Authentication;
using FCSP.Services.User;

namespace FCSP.WebAPI.Configuration;

internal static class ServiceConfig
{
    #region Public methods
    public static void Configure(IServiceCollection services)
    {
        RegisterServices(services);
    }
    #endregion

    #region Private methods
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ITokenService, JwtService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
    }
    #endregion
}
