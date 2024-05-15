using System.ComponentModel.DataAnnotations;

namespace ECommWeb.Core.src.Entity;

public class Product : BaseEntity
{
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Product price is required")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Price must be larger than 0")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Product description is required")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Product inventory is required")]
    [Range(0, Int32.MaxValue, ErrorMessage = "Inventory must be larger than or equal to 0")]
    public int Inventory { get; set; }
    [Required(ErrorMessage = "Product weight is required")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Weight must be larger than 0")]
    public double Weight { get; set; }
    [Required(ErrorMessage = "Product category id is required")]
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } // navigation A product belongs to one category
    public IEnumerable<ProductImage> Images { get; set; } // relationship A product has a list of Images

}