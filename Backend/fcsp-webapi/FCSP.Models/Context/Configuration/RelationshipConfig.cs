using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Models.Context.Configuration;

internal static class RelationshipConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShippingInfo>()
            .HasOne(shippingInfo => shippingInfo.User)
            .WithMany(user => user.ShippingInfos)
            .HasForeignKey(shippingInfo => shippingInfo.UserId);
            
        // Configure relationship for CustomShoeDesign and CustomShoeDesignTemplate
        modelBuilder.Entity<CustomShoeDesign>()
            .HasOne(d => d.CustomShoeDesignTemplate)
            .WithMany(t => t.CustomShoeDesigns)
            .HasForeignKey(d => d.CustomShoeDesignTemplateId);

        // Skip explicit configuration for DesignService for now
        // This will use conventional configuration based on property names
        // We'll need to create a custom value converter if we want to map
        // between string/float and bigint types
    }
}
