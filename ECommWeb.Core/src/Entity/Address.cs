using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommWeb.Core.src.Entity;

public class Address : BaseEntity
{
    [Required(ErrorMessage = "Address details is required")]
    [StringLength(50, ErrorMessage = "50 characters at most.")]
    public string? AddressLine { get; set; }

    [Column(TypeName = "character varying(50)")]
    [Required(ErrorMessage = "Street is required")]
    public string Street { get; set; }

    [Required(ErrorMessage = "City is required")]
    [Column(TypeName = "character varying(20)")]
    public string City { get; set; }

    [Required(ErrorMessage = "Country is required")]
    [Column(TypeName = "character varying(50)")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Postcode is required")]
    [PostalCode(ErrorMessage = "Invalid postal code")]
    public string Postcode { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; }

    [StringLength(100, ErrorMessage = "Landmark cannot be longer than 100 characters")]
    public string Landmark { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}
