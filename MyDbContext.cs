using IsitechEfCoreApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace IsitechEfCoreApp;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            // Hierarchical categorization
            entity.HasOne(c => c.ParentCategory)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation 1 -> n
            entity.HasMany(c => c.Products)
                  .WithOne(p => p.Category)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).HasColumnName("ProductName");

            // Stock management is handled via StockQuantity
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            
            entity.HasIndex(c => c.Email).IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId);

            entity.HasOne(o => o.ShippingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.ShippingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.BillingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.BillingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId);

            entity.HasOne(oi => oi.Product)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.ToTable("ProductReviews");

            entity.HasOne(pr => pr.Product)
                  .WithMany(p => p.ProductReviews)
                  .HasForeignKey(pr => pr.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pr => pr.Customer)
                  .WithMany(c => c.ProductReviews)
                  .HasForeignKey(pr => pr.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.ToTable("PriceHistories");

            entity.HasOne(ph => ph.Product)
                  .WithMany(p => p.PriceHistory)
                  .HasForeignKey(ph => ph.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}