using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IsitechEfCoreApp.Entities;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = null!;

    [Required, StringLength(100)]
    public string Email { get; set; } = null!;

    // New relationships
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
