using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Order
{
  [Key]
  public int OrderId { get; set; }

  [Required]
  public DateTime OrderDate { get; set; }

  // Clé étrangère vers Customer
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;

  // Relation 1-n vers OrderItem
  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}