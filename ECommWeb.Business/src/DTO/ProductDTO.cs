using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class ProductReadDTO : BaseEntity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public CategoryReadDTO Category { get; set; }
    public IEnumerable<ProductImageReadDTO> Images { get; set; }
    public int Inventory { get; set; }
    public double Weight { get; set; }

}

public class ProductCreateDTO
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageCreateDTO> Images { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductUpdateDTO
{
    public double? Price { get; set; }
    public int Inventory { get; set; }
}