using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Models.Context;

internal static class RelationshipConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShippingInfo>()
            .HasOne(shippingInfo => shippingInfo.User)
            .WithMany(user => user.ShippingInfos)
            .HasForeignKey(shippingInfo => shippingInfo.UserId);
    }
}
