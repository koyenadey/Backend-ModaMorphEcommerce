using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using Microsoft.AspNetCore.Authorization;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryServices;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryServices = categoryService;
    }


    [AllowAnonymous]
    [HttpGet("api/v1/categories")]
    public async Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetAllCategoriesAsync([FromQuery] QueryOptions options)
    {
        var result = await _categoryServices.GetAllCategoriesAsync(options);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("api/v1/category/{id}")]
    public async Task<ActionResult<CategoryReadDTO>> GetCategoryByIdAsync([FromRoute] Guid id)
    {
        var result = await _categoryServices.GetCategoryById(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("api/v1/category")]
    public async Task<ActionResult<CategoryReadDTO>> CreateCategoryAsync([FromBody] CategoryCreateDTO category)
    {
        var result = await _categoryServices.CreateCategory(category);
        if (result == null) return BadRequest("Category could not be created");
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("api/v1/category/{id}")]
    public async Task<ActionResult<CategoryReadDTO>> UpdateCategoryAsync([FromRoute] Guid id, [FromBody] CategoryUpdateDTO category)
    {
        var result = await _categoryServices.UpdateACategory(id, category);
        if (result == null) return BadRequest("Category could not be updated");
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("api/v1/category/{id}")]
    public async Task<ActionResult<bool>> DeleteCategoryAsync([FromRoute] Guid id)
    {
        var isDeleted = await _categoryServices.DeleteCategory(id);
        if (!isDeleted) return BadRequest("Category could not be deleted");
        return Ok(isDeleted);
    }
}
