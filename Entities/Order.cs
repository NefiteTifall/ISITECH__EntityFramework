using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Order
{
	[Key]
	public int OrderId { get; set; }

	[Required]
	public DateTime OrderDate { get; set; }

	// Foreign keys for addresses
	public int CustomerId { get; set; }
	public Customer Customer { get; set; } = null!;

	public int ShippingAddressId { get; set; }
	public Address ShippingAddress { get; set; } = null!;

	public int BillingAddressId { get; set; }
	public Address BillingAddress { get; set; } = null!;

	// Relation 1-n vers OrderItem
	public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}