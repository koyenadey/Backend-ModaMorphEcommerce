using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Business.src.DTO;

public class WishlistReadItemDto
{
    public Guid Id { get; set; }
    public ProductReadDTO Product { get; set; }
}

public class WishlistCreateItemDto
{
    Guid ProductId { get; set; }
    Guid WishlistId { get; set; }
}
