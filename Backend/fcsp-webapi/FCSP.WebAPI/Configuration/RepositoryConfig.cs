using FCSP.Models.Context;
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
        services.AddScoped<IPostsCommentsRepository, PostsCommentsRepository>();

        // Cart repositories
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();

        // Design repositories
        services.AddScoped<ICustomShoeDesignRepository, CustomShoeDesignRepository>();
        services.AddScoped<ICustomShoeDesignTextureRepository, CustomShoeDesignTextureRepository>();
        services.AddScoped<ICustomShoeDesignTexturesRepository, CustomShoeDesignTexturesRepository>();
        services.AddScoped<ICustomShoeDesignTemplateRepository, TemplateRepository>();
        services.AddScoped<IDesignPreviewRepository, DesignPreviewRepository>();
        services.AddScoped<IDesignServiceRepository, DesignServiceRepository>();
        services.AddScoped<IReturnedCustomShoeRepository, ReturnedCustomShoeRepository>();
        
        // Order & Payment repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentGatewayRepository, PaymentGatewayRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        
        // Service repositories
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISetServiceAmountRepository, SetServiceAmountRepository>();
        // Texture & Rating repositories
        services.AddScoped<ITextureRepository, TextureRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        
        // User Activity & Recommendation repositories
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IUserRecommendationRepository, UserRecommendationRepository>();
        
        // Notification repository
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Manufacturer repository
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

        // Shipping repository
        services.AddScoped<IShippingInfoRepository, ShippingInfoRepository>();
        
        // Size repository
        services.AddScoped<ISizeRepository, SizeRepository>();
    }
}
