using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class ProductReview
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
