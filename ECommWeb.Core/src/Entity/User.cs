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

        // foreign key
        public Guid DefaultAddressId { get; set; }

        public byte[] Salt { get; set; } // random key to hash password

        // relation - a user has a list of addresses
        public IEnumerable<Address> Addresses { get; set; }

        // wishlist - a user has a list of wishlists
        //public IEnumerable<Wishlist> Wishlists { get; set; }

        public User(string userName, string email, string password, byte[] salt, Role role = Role.Customer)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            Password = password;
            Role = role;
            Salt = salt;
        }
        public User(string userName, string email, string password, Role role = Role.Customer)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            Password = password;
            Role = role;
        }
    }

}