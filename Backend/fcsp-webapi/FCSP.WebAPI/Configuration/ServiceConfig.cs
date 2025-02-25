using FCSP.Services.Auth;
using FCSP.Services.Auth.Hash;
using FCSP.Services.Auth.Token;
using FCSP.Services.CustomShoeDesignService;
using FCSP.Services.CustomShoeDesignTextureService;
using FCSP.Services.OrderDetailService;
using FCSP.Services.OrderService;
using FCSP.Services.PaymentGatewayService;
using FCSP.Services.PaymentService;
using FCSP.Services.PostCommentService;
using FCSP.Services.PostService;
using FCSP.Services.ShippingInfoService;
using FCSP.Services.TemplateService;
using FCSP.Services.TextureService;
using FCSP.Services.UserTextureService;
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
        // Authentication services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<ITokenService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        
        // Design services
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ICustomShoeDesignService, CustomShoeDesignService>();
        services.AddScoped<ICustomShoeDesignTextureService, CustomShoeDesignTextureService>();
        
        // Texture services
        services.AddScoped<ITextureService, TextureService>();
        services.AddScoped<IUserTextureService, UserTextureService>();
        
        // Order services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        
        // Payment services
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        
        // Post services
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IPostCommentService, PostCommentService>();
        
        // Shipping services
        services.AddScoped<IShippingInfoService, ShippingInfoService>();
    }
}
