using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Core.src.Entity;

public class WishlistItem : BaseEntity
{
    // foreign key
    public Guid ProductId { get; set; }
    // navigation
    public Product Product { get; set; }
    //foreign key
    public Guid WishlistId { get; set; }
    // navigation
    public Wishlist Wishlist { get; set; }
}
