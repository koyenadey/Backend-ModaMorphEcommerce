using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommWeb.Core.src.Entity;

public class Address : BaseEntity
{
    [Required(ErrorMessage = "Address details is required")]
    [StringLength(50, ErrorMessage = "50 characters at most.")]
    public string AddressLine { get; set; }
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

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; }

    [StringLength(100, ErrorMessage = "Landmark cannot be longer than 100 characters")]
    public string Landmark { get; set; }
    // foreign key
    public Guid UserId { get; set; }
    // navigation
    public User User { get; set; }
    public Address(string addressLine, string street, string city, string country, string postcode, string phoneNumber, string firstName, string lastName, string landmark, Guid userId)
    {
        Id = Guid.NewGuid();
        AddressLine = addressLine;
        Street = street;
        City = city;
        Country = country;
        Postcode = postcode;
        PhoneNumber = phoneNumber;
        FirstName = firstName;
        LastName = lastName;
        Landmark = landmark;
        UserId = userId;
    }
}
