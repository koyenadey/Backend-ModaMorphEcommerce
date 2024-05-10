using System.ComponentModel.DataAnnotations;

namespace ECommWeb.Core.src.Entity;

public class Product : BaseEntity
{
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Product price is required")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Price must be larger than 0")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Product description is required")]
    public string Description { get; set; }
    public IEnumerable<ProductImage> Images { get; set; }
    [Required(ErrorMessage = "Product inventory is required")]
    [Range(0, Int32.MaxValue, ErrorMessage = "Inventory must be larger than or equal to 0")]
    public int Inventory { get; set; }
    [Required(ErrorMessage = "Product weight is required")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Weight must be larger than 0")]
    public decimal Weight { get; set; }
    [Required(ErrorMessage = "Product category id is required")]
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public IEnumerable<ProductImage> ProductImages { get; set; }
    public Product(string name, decimal price, string description, int inventory, decimal weight, Guid categoryId)
    {
        Name = name;
        Price = price;
        Description = description;
        Inventory = inventory;
        Weight = weight;
        CategoryId = categoryId;
        // Category = category;
    }
}