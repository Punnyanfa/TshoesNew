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
    public FcspDbContext(DbContextOptions<FcspDbContext> options) : base(options)
    {
    }

    public FcspDbContext()
    {
    }
    #endregion

    #region Properties
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<CustomShoeDesign> CustomShoeDesigns { get; set; }
    public DbSet<CustomShoeDesignTemplate> CustomShoeDesignTemplates { get; set; }
    public DbSet<CustomShoeDesignTextures> CustomShoeDesignTextures { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ShippingInfo> ShippingInfos { get; set; }
    public DbSet<Texture> Textures { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Posts> Posts { get; set; }
    public DbSet<PostsComments> PostsComments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<DesignService> DesignServices { get; set; }
    public DbSet<DesignPreview> DesignPreviews { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<SetServiceAmount> SetServiceAmounts { get; set; }
    public DbSet<Designer> Designers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<ReturnedCustomShoe> ReturnedCustomShoes { get; set; }
    public DbSet<Size> Sizes { get; set; }
    #endregion

    #region Private Methods
    private static string? GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", false)
            .Build();

        return config.GetConnectionString("FCSP_DB_SOMEE");
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
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
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
