using IsitechEfCoreApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace IsitechEfCoreApp;

public class MyDbContext : DbContext
{
	public MyDbContext(DbContextOptions<MyDbContext> options)
		: base(options)
	{
	}

	// On déclare les DbSet qui correspondent aux tables
	public DbSet<TodoItem> Todos => Set<TodoItem>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();
	public DbSet<Customer> Customers => Set<Customer>();
	public DbSet<Product> Products => Set<Product>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>(entity =>
		{
			entity.ToTable("Categories");

			// Relation 1 -> n
			entity.HasMany(c => c.Products)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		// --- Product ---
		modelBuilder.Entity<Product>(entity =>
		{
			entity.Property(p => p.Name).HasColumnName("ProductName");
		});

		// --- Customer ---
		modelBuilder.Entity<Customer>(entity =>
		{
			entity.ToTable("Customers");
			
			entity.HasIndex(c => c.Email).IsUnique();
		});

		// --- Order ---
		modelBuilder.Entity<Order>(entity =>
		{
			entity.ToTable("Orders");
			
			entity.HasOne(o => o.Customer)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.CustomerId);
		});

		// --- OrderItem ---
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
	}
}