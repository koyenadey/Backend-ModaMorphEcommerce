using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryServices;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryServices = categoryService;
    }

    [HttpGet("api/v1/categories")]
    public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync([FromQuery] QueryOptions options)
    {
        Console.WriteLine("GetAllCategoriesAsync");
        try
        {
            return await _categoryServices.GetAllCategoriesAsync(options);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("api/v1/category/{id}")]
    public async Task<CategoryReadDTO> GetCategoryByIdAsync([FromRoute] Guid id)
    {
        return await _categoryServices.GetCategoryById(id);
    }

    [HttpPost("api/v1/category/{id}")]
    public async Task<CategoryReadDTO> CreateCategoryAsync([FromBody] CategoryCreateDTO category)
    {
        return await _categoryServices.CreateCategory(category);
    }
    [HttpPatch("api/v1/category/{id}")]
    public async Task<CategoryReadDTO> UpdateCategoryAsync([FromRoute] Guid id, [FromBody] CategoryUpdateDTO category)
    {
        return await _categoryServices.UpdateCategory(id, category);
    }
    [HttpDelete("api/v1/category/{id}")]
    public async Task<bool> DeleteCategoryAsync([FromRoute] Guid id)
    {
        return await _categoryServices.DeleteCategory(id);
    }
}
