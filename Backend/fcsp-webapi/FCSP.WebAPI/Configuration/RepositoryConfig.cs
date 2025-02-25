using FCSP.Models.Context;
using FCSP.Repositories;
using FCSP.Repositories.Implementations;
using FCSP.Repositories.Interfaces;

namespace FCSP.WebAPI.Configuration;

internal static class RepositoryConfig
{
    public static void Configure(IServiceCollection services)
    {
        RegisterRepositories(services);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        // Register DbContext
        services.AddDbContext<FcspDbContext>();
        
        // User repositories
        services.AddScoped<IUserRepository, UserRepository>();
        
        // Post & Comment repositories
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();
        
        // Texture repositories
        services.AddScoped<ITextureRepository, TextureRepository>();
        services.AddScoped<IUserTextureRepository, UserTextureRepository>();
        
        // Design repositories
        services.AddScoped<ICustomShoeDesignRepository, CustomShoeDesignRepository>();
        services.AddScoped<ICustomShoeDesignTextureRepository, CustomShoeDesignTextureRepository>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        
        // Order repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        
        // Payment repositories
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentGatewayRepository, PaymentGatewayRepository>();
        
        // Shipping repositories
        services.AddScoped<IShippingInfoRepository, ShippingInfoRepository>();
    }
}
