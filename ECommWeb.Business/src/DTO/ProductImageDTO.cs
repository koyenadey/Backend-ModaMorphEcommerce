using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class ProductImageReadDTO : BaseEntity
{
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; }
}

public class ProductOrderImageReadDTO : BaseEntity
{
    public string ImageUrl { get; set; }
}

public class ProductImageCreateDTO
{
    public string ImageUrl { get; set; }
}

public class ProductImageUpdateDTO
{
    public string ImageUrl { get; set; }
}