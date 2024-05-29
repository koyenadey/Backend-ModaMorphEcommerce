using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Core.src.Entity
{
    public class Wishlist : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        // foreign key
        public Guid UserId { get; set; }
        public User User { get; set; }

        // relation - a wishlist contains a list of wishlist items
        public IEnumerable<WishlistItem> WishlistItems { get; set; }

    }
}