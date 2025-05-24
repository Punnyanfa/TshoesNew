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
            .WithOne()
            .HasForeignKey<User>(u => u.DefaultAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShippingInfo>()
            .HasOne(shippingInfo => shippingInfo.User)
            .WithMany(user => user.ShippingInfos)
            .HasForeignKey(shippingInfo => shippingInfo.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // UserOtp relationships
        modelBuilder.Entity<UserOtp>()
            .HasOne(otp => otp.User)
            .WithMany()
            .HasForeignKey(otp => otp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Cart relationships
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // CartItem relationships
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.CustomShoeDesign)
            .WithMany(cd => cd.CartItems)
            .HasForeignKey(ci => ci.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Size)
            .WithMany()
            .HasForeignKey(ci => ci.SizeId)
            .OnDelete(DeleteBehavior.Restrict);
            
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

        // CustomShoeDesignTexture relationships
        modelBuilder.Entity<CustomShoeDesignTextures>()
            .HasOne(dt => dt.CustomShoeDesign)
            .WithMany(d => d.CustomShoeDesignTextures)
            .HasForeignKey(dt => dt.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CustomShoeDesignTextures>()
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
            .WithMany(v => v.Orders)
            .HasForeignKey(o => o.VoucherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingInfo)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.ShippingInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderDetail relationships
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.CustomShoeDesign)
            .WithMany(d => d.OrderDetails)
            .HasForeignKey(od => od.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.NoAction); // Use NoAction to avoid multiple cascade paths

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Manufacturer)
            .WithMany(m => m.OrderDetails)
            .HasForeignKey(od => od.ManufacturerId)
            .OnDelete(DeleteBehavior.NoAction); // Use NoAction to avoid multiple cascade paths

        // Payment relationships
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rating relationships
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.CustomShoeDesign)
            .WithMany(d => d.Ratings)
            .HasForeignKey(r => r.CustomShoeDesignId)
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
            .HasForeignKey(ds => ds.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DesignService>()
            .HasOne(ds => ds.Service)
            .WithMany(s => s.DesignServices)
            .HasForeignKey(ds => ds.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Manufacturer relationships
        modelBuilder.Entity<Manufacturer>()
            .HasOne(m => m.User)
            .WithMany(u => u.Manufacturers)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Service relationships
        modelBuilder.Entity<Service>()
            .HasOne(s => s.Manufacturer)
            .WithMany(m => m.Services)
            .HasForeignKey(s => s.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Designer relationships
        modelBuilder.Entity<Designer>()
            .HasOne(d => d.User)
            .WithMany(u => u.Designers)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Transaction relationships
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Receiver)
            .WithMany(u => u.ReceivedTransactions)
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.OrderDetail)
            .WithMany(od => od.Transactions)
            .HasForeignKey(t => t.OrderDetailId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Payment)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);

        // Size relationships
        modelBuilder.Entity<Size>()
            .HasMany(s => s.OrderDetails)
            .WithOne(d => d.Size)
            .HasForeignKey(d => d.SizeId)
            .OnDelete(DeleteBehavior.Restrict);

        // ReturnedCustomShoe relationships
        modelBuilder.Entity<ReturnedCustomShoe>()
            .HasOne(r => r.CustomShoeDesign)
            .WithMany(d => d.ReturnedCustomShoes)
            .HasForeignKey(r => r.CustomShoeDesignId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
