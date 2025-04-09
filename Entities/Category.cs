using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Category
{
	[Key]
	public int CategoryId { get; set; }

	[Required, StringLength(50)]
	public string Name { get; set; } = null!;

	// Self-referencing relationship for hierarchy
	public int? ParentCategoryId { get; set; }
	public Category? ParentCategory { get; set; }

	public ICollection<Category> SubCategories { get; set; } = new List<Category>();
	public ICollection<Product> Products { get; set; } = new List<Product>();
}