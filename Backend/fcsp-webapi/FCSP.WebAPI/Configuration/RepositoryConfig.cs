using FCSP.Models.Context;
using FCSP.Repositories;

namespace FCSP.WebAPI.Configuration;

internal static class RepositoryConfig
{
    public static void Configure(IServiceCollection services)
    {
        RegisterRepositories(services);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<FcspDbContext>();
    }
}
