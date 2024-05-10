using System.ComponentModel.DataAnnotations;

namespace ECommWeb.Core.src.Entity;

public class Category : BaseEntity
{
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Category image is required")]
    public string Image { get; set; }
    public Guid? Parent_id { get; set; }
    public IEnumerable<Product>? Products { get; set; }
    public Category(string name, string image)
    {
        Name = name;
        Image = image;
        // Parent_id = parent_id;
    }
}