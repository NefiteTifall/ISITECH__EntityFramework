using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsitechEfCoreApp.Entities;

[Table("OrderItems")] 
public class OrderItem
{
  [Key]
  public int OrderItemId { get; set; }

  // Clés étrangères
  public int OrderId { get; set; }
  public Order Order { get; set; } = null!;

  public int ProductId { get; set; }
  public Product Product { get; set; } = null!;

  [Required]
  [Range(1, Int32.MaxValue)]
  public int Quantity { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal UnitPrice { get; set; }
}