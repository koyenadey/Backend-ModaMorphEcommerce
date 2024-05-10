using System.ComponentModel.DataAnnotations;

namespace ECommWeb.Core.src.Entity;

public class ProductImage : BaseEntity
{
    [Required(ErrorMessage = "Product id is required")]
    public Guid ProductId { get; set; }
    // navigation
    public Product Product { get; set; }
    [Required(ErrorMessage = "Product image url is required")]
    public string Url { get; set; }
}