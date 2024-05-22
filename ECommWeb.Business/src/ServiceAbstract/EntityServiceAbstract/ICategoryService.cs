using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface ICategoryService
{
    Task<int> GetProductsCount();
    public Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(QueryOptions options);
    public Task<CategoryReadDTO> GetCategoryById(Guid id);
    public Task<CategoryReadDTO> CreateCategory(CategoryCreateDTO category);
    public Task<CategoryReadDTO> UpdateACategory(Guid id, CategoryUpdateDTO category);
    public Task<bool> DeleteCategory(Guid id);
}
