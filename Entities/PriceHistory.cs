using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsitechEfCoreApp.Entities;

public class PriceHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public DateTime EffectiveDate { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
