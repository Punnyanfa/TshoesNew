using FCSP.Models.Context.Configuration;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FCSP.Models.Context;

public class FcspDbContext : DbContext
{
    #region Fields
    #endregion

    #region Constructors
    #endregion

    #region Properties
    public DbSet<CustomShoeDesign> CustomShoeDesigns { get; set; }
    public DbSet<CustomShoeDesignTemplate> CustomShoeDesignTemplates { get; set; }
    public DbSet<CustomShoeDesignTexture> CustomShoeDesignTextures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentGateway> PaymentGateways { get; set; }
    public DbSet<ShippingInfo> ShippingInfos { get; set; }
    public DbSet<Texture> Textures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    #endregion

    #region Private Methods
    private static string? GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false)
            .Build();

        return config.GetConnectionString("FCSP_DB");
    }

    private static void ConfigureModel(ModelBuilder modelBuilder)
    {
        EnumConfig.Configure(modelBuilder);
        RelationshipConfig.Configure(modelBuilder);
        DataConfig.Configure(modelBuilder);
    }
    #endregion

    #region Protected Methods
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureModel(modelBuilder);
    }
    #endregion

    #region Public Methods
    #endregion
}
