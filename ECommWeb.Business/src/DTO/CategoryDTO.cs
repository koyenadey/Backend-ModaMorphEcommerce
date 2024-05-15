using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class CategoryReadDTO : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }

}

public class CategoryCreateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
    public Guid? ParentId { get; set; }
}

public class CategoryUpdateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
}