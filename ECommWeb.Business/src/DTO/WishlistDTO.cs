using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class WishlistReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public UserReadDto User { get; set; }
    public IEnumerable<WishlistReadItemDto> WishlistItems { get; set; }
}

public class WishlistCreateDto
{
    public string Name { get; set; }
    public Guid UserId { get; set; }
}

public class AddToWishlistDto
{
    public Guid ProductId { get; set; }
}
public class WishlistUpdateDto
{
    public string Name { get; set; }
}
