using System.ComponentModel.DataAnnotations;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Core.src.Entity
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        public Role Role { get; set; }

        [MaxLength(255)]
        public string Avatar { get; set; }
        public Guid DefaultAddressId { get; set; }
        public byte[] Salt { get; set; } // random key to hash password
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }

}