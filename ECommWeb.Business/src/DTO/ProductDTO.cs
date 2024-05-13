using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class ProductReadDTO : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageReadDTO> Images { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public CategoryReadDTO Category { get; set; }
    public void Transform(Product product)
    {
        product.Name = Name;
        product.Price = Price;
        product.Description = Description;
        product.Inventory = Inventory;
        product.Weight = Weight;
        product.CategoryId = Category.Id;
    }
}

public class ProductCreateDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageCreateDTO> Images { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
}

public class ProductUpdateDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public IEnumerable<ProductImageCreateDTO>? Images { get; set; }
    public int Inventory { get; set; }
    public decimal Weight { get; set; }
    public Guid CategoryId { get; set; }
    public Product UpdateProduct(Product oldProduct)
    {
        oldProduct.Name = Name;
        oldProduct.Price = Price;
        oldProduct.Description = Description;
        oldProduct.Inventory = Inventory;
        oldProduct.Weight = Weight;
        oldProduct.CategoryId = CategoryId;
        return oldProduct;
    }
}