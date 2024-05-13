using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class CategoryReadDTO : BaseEntity
{
    public string Name { get; set; }
    public string Image { get; set; }
    public void Transform(Category category)
    {
        category.Name = Name;
        category.Image = Image;
    }
}

public class CategoryCreateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
}

public class CategoryUpdateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Category UpdateCategory(Category oldCate)
    {
        oldCate.Name = Name;
        oldCate.Image = Image;
        return oldCate;
    }
}