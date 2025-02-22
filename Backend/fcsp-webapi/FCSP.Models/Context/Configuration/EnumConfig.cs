using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Models.Context.Configuration;

internal static class EnumConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Order>()
            .Property(o => o.ShippingStatus)
            .HasConversion<int>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasConversion<int>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentStatus)
            .HasConversion<int>();

        modelBuilder.Entity<PaymentGateway>()
            .Property(p => p.PaymentMethod)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(p => p.UserRole)
            .HasConversion<int>();
    }
}
