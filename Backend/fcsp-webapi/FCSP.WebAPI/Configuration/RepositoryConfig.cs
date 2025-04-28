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
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IUserRecommendationRepository, UserRecommendationRepository>();
        
        // Social (Posts & Comments) repositories
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
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        
        // Service & Manufacturer repositories
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISetServiceAmountRepository, SetServiceAmountRepository>();
        services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
        services.AddScoped<IManufacturerCriteriaRepository, ManufacturerCriteriaRepository>();
        services.AddScoped<ICriteriaRepository, CriteriaRepository>();
        
        // Texture & Rating repositories
        services.AddScoped<ITextureRepository, TextureRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        
        // Notification repository
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Shipping & Size repositories
        services.AddScoped<IShippingInfoRepository, ShippingInfoRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        
        // Designer repository
        services.AddScoped<IDesignerRepository, DesignerRepository>();
    }
}
