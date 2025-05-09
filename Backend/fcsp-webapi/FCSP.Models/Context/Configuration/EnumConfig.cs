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

        modelBuilder.Entity<CustomShoeDesignTemplate>()
            .Property(c => c.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasConversion<int>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentStatus)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(p => p.UserRole)
            .HasConversion<int>();

        // Configure Status properties as enums stored as int
        modelBuilder.Entity<Manufacturer>()
            .Property(m => m.Status)
            .HasConversion<int>();

        modelBuilder.Entity<SetServiceAmount>()
            .Property(ssa => ssa.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Designer>()
            .Property(d => d.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Voucher>()
            .Property(v => v.Status)
            .HasConversion<int>();
    }
}
