using System.ComponentModel.DataAnnotations;

namespace IsitechEfCoreApp.Entities;

public class Address
{
	[Key]
	public int AddressId { get; set; }

	[Required]
	public AddressType AddressType { get; set; }

	[Required]
	public string AddressLine1 { get; set; } = null!;

	public string? AddressLine2 { get; set; }

	[Required]
	public string City { get; set; } = null!;

	[Required]
	public string StateProvince { get; set; } = null!;

	[Required]
	public string PostalCode { get; set; } = null!;

	[Required]
	public string Country { get; set; } = null!;
}

public enum AddressType
{
	Shipping,
	Billing,
}