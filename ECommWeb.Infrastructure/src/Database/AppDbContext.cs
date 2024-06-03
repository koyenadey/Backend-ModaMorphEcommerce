using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Infrastructure.src.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } // table `Users` -> `users`
    public DbSet<Address> Addresses { get; set; } // table `Addresses` -> `addresses`
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; } // table `Orders` -> `orders`
    public DbSet<OrderProduct> OrderedProducts { get; set; } // table `Orders` -> `orders`
    public DbSet<Review> Reviews { get; set; } // table `Reviews` -> `reviews`
    public DbSet<ReviewImage> ReviewImages { get; set; } // table `Reviews` -> `reviews`
    public DbSet<Wishlist> Wishlists { get; set; } // table `Wishlists` -> `wishlists`
    public DbSet<WishlistItem> WishlistItems { get; set; } // table `WishlistItems` -> `wishlistitems`


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    static AppDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(new TimeStampInteceptor())
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Role>();
        modelBuilder.HasPostgresEnum<PaymentMethod>();
        modelBuilder.HasPostgresEnum<Status>();

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasMany(u => u.Addresses)
                  .WithOne(a => a.User)
                  .HasForeignKey(a => a.UserId);
            entity.HasMany(u => u.Orders)
                  .WithOne(o => o.User)
                  .HasForeignKey(o => o.UserId);
            entity.HasMany(u => u.Wishlists)
                  .WithOne(w => w.User)
                  .HasForeignKey(w => w.UserId);
            entity.HasMany(u => u.Reviews)
                  .WithOne(r => r.User)
                  .HasForeignKey(r => r.UserId);
            //entity.HasData(SeedingData.GetUsers());
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(a => a.User)
                  .WithMany(u => u.Addresses)
                  .HasForeignKey(a => a.UserId);
            //entity.HasData(SeedingData.GetAddresses());
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Category>(e =>
            {
                //e.HasData(SeedingData.GetCategories());
                e.HasIndex(e => e.Name).IsUnique();
            });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Product>(e =>
        {
            //e.HasData(SeedingData.Products);

            e.HasIndex(p => p.Name).IsUnique();
            e.HasMany(p => p.Images) // Product has many ProductImage
            .WithOne()              // ProductImage bonds to one Product
        .HasForeignKey(pi => pi.ProductId);

            e.HasMany(p => p.OrderedProducts)
            .WithOne(op => op.Product)
            .HasForeignKey(op => op.ProductId);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<ProductImage>(e =>
        {
            e.HasOne(pi => pi.Product)   // Configure the navigation property to Product
            .WithMany(p => p.Images) // Product has many ProductImages
            .HasForeignKey(pi => pi.ProductId) // Set the foreign key
            .IsRequired(); // Ensure that the relationship is required (optional)
            //e.HasData(SeedingData.GetProductImages()); // Seed the product images
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Order>(e =>
        {
            //User-Order relationship
            e.HasOne(o => o.User)   // Configure the navigation property to User
                .WithMany(u => u.Orders) // A User has many Order
                .HasForeignKey(u => u.UserId) // Set the foreign key
                .IsRequired(); // Ensure that the relationship is required (optional)

            //Order-Address relationship   
            e.HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId);

            //Order-OrderedProduct relationship   
            e.HasMany(o => o.OrderedProducts)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);

            //e.HasData(SeedingData.GetOrders()); // Seed the order data
        });
        // -----------------------------------------------------------------------------------------------
        //OrderProduct-Product relationship
        modelBuilder.Entity<OrderProduct>(e =>
        {
            e.HasKey(op => op.Id);
            e.HasIndex(op => new { op.OrderId, op.ProductId }).IsUnique();
            e.HasOne(op => op.Product)
                .WithMany(p => p.OrderedProducts)
                .HasForeignKey(op => op.ProductId);

            e.HasMany(op => op.Reviews)
                .WithOne(r => r.OrderedProduct)
                .HasForeignKey(r => r.OrderedProductId);
            //e.HasData(SeedingData.GetOrderedProducts());
        });
        // -----------------------------------------------------------------------------------------------

        modelBuilder.Entity<Review>(e =>
        {
            //Review-OrderedProduct relationship
            e.HasOne(r => r.OrderedProduct)
                .WithMany(op => op.Reviews)
                .HasForeignKey(r => r.OrderedProductId);

            //Review-User relationship
            e.HasOne(u => u.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(u => u.UserId);

            //Review-ReviewImages relationship
            e.HasMany(r => r.Images)
                .WithOne()
                .HasForeignKey(r => r.ReviewId);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<ReviewImage>(entity =>
        {
            entity.HasKey(ri => ri.Id);
            entity.HasOne(ri => ri.Review)
                  .WithMany(r => r.Images)
                  .HasForeignKey(ri => ri.ReviewId);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasIndex(wl => new { wl.Name, wl.UserId }).IsUnique();
            entity.HasMany(wl => wl.WishlistItems)
                .WithOne(wli => wli.Wishlist)
                .HasForeignKey(wli => wli.WishlistId);
            //entity.HasData(SeedingData.Wishlists);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasIndex(wli => new { wli.ProductId, wli.WishlistId }).IsUnique();
            entity.HasOne(wl => wl.Product)
                .WithMany(p => p.WishlistItems)
                .HasForeignKey(wli => wli.ProductId);
            entity.HasOne(wli => wli.Wishlist)
                .WithMany(wl => wl.WishlistItems)
                .HasForeignKey(wli => wli.WishlistId);
            //entity.HasData(SeedingData.WishlistItems);
        });


        base.OnModelCreating(modelBuilder);
    }
}
