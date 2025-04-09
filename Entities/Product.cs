using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsitechEfCoreApp.Entities;

[Table("Products")] // On nomme la table au moyen d'un Data Annotation
public class Product
{
  [Key]
  public int ProductId { get; set; }

  [Required, StringLength(100)]
  public string Name { get; set; } = null!;

  [Column(TypeName = "decimal(18,2)")]
  [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
  public decimal Price { get; set; }

  // Clé étrangère vers Category
  [ForeignKey(nameof(Category))]
  public int CategoryId { get; set; }
  public Category Category { get; set; } = null!;
  public int StockQuantity { get; set; }
  
  public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
  public ICollection<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
}