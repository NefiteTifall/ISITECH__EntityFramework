using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Customer
{
  [Key]
  public int CustomerId { get; set; }

  [Required, StringLength(50)]
  public string FirstName { get; set; } = null!;

  [Required, StringLength(50)]
  public string LastName { get; set; } = null!;

  [Required, StringLength(100)]
  public string Email { get; set; } = null!;

  // Relation 1-n avec Order
  public ICollection<Order> Orders { get; set; } = new List<Order>();
}