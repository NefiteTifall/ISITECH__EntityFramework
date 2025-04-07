using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Category
{
  [Key]
  public int CategoryId { get; set; }

  [Required, StringLength(50)]
  public string Name { get; set; } = null!;

  // Relation 1-n avec Product
  public ICollection<Product> Products { get; set; } = new List<Product>();
}