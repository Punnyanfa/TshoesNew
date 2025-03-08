using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Models.Context.Configuration;

internal static class RelationshipConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        // User relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.DefaultAddress)
            .WithMany()
            .HasForeignKey(u => u.DefaultAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShippingInfo>()
            .HasOne(shippingInfo => shippingInfo.User)
            .WithMany(user => user.ShippingInfos)
            .HasForeignKey(shippingInfo => shippingInfo.UserId);
            
        // CustomShoeDesign relationships
        modelBuilder.Entity<CustomShoeDesign>()
            .HasOne(d => d.CustomShoeDesignTemplate)
            .WithMany(t => t.CustomShoeDesigns)
            .HasForeignKey(d => d.CustomShoeDesignTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomShoeDesign>()
            .HasOne(d => d.User)
            .WithMany(u => u.CustomShoeDesigns)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // CustomShoeDesignTemplate relationships
        modelBuilder.Entity<CustomShoeDesignTemplate>()
            .HasOne(t => t.User)
            .WithMany(u => u.Templates)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // CustomShoeDesignTexture relationships
        modelBuilder.Entity<CustomShoeDesignTexture>()
            .HasOne(dt => dt.CustomShoeDesign)
            .WithMany(d => d.CustomShoeDesignTextures)
            .HasForeignKey(dt => dt.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomShoeDesignTexture>()
            .HasOne(dt => dt.Texture)
            .WithMany(t => t.CustomShoeDesignTextures)
            .HasForeignKey(dt => dt.TextureId)
            .OnDelete(DeleteBehavior.Restrict);

        // Texture relationships
        modelBuilder.Entity<Texture>()
            .HasOne(t => t.User)
            .WithMany(u => u.Textures)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Posts relationships
        modelBuilder.Entity<Posts>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // PostsComments relationships
        modelBuilder.Entity<PostsComments>()
            .HasOne(pc => pc.User)
            .WithMany(u => u.PostsComments)
            .HasForeignKey(pc => pc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PostsComments>()
            .HasOne(pc => pc.Posts)
            .WithMany(p => p.PostsComments)
            .HasForeignKey(pc => pc.PostsId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order relationships
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Voucher)
            .WithMany()
            .HasForeignKey(o => o.VoucherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingInfo)
            .WithMany()
            .HasForeignKey(o => o.ShippingInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderDetail relationships
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment relationships
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // PaymentGateway relationships
        modelBuilder.Entity<PaymentGateway>()
            .HasOne(pg => pg.User)
            .WithMany(u => u.PaymentGateways)
            .HasForeignKey(pg => pg.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification relationships
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserActivity relationships
        modelBuilder.Entity<UserActivity>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserRecommendation relationships
        modelBuilder.Entity<UserRecommendation>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Recommendations)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rating relationships
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // DesignPreview relationships
        modelBuilder.Entity<DesignPreview>()
            .HasOne(dp => dp.CustomShoeDesign)
            .WithMany(d => d.DesignPreviews)
            .HasForeignKey(dp => dp.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);

        // DesignService relationships
        modelBuilder.Entity<DesignService>()
            .HasOne(ds => ds.CustomShoeDesign)
            .WithMany(d => d.DesignServices)
            .HasForeignKey(ds => ds.DesignId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DesignService>()
            .HasOne(ds => ds.Service)
            .WithMany()
            .HasForeignKey(ds => ds.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
