using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
[Route("api/v1/images")]
public class ProductImageController : ControllerBase
{
    private readonly IProductImageService _productImageService;

    public ProductImageController(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    [HttpGet("/{productId}")]
    public async Task<IEnumerable<ProductImageReadDTO>> GetAllProductImagesAsync([FromQuery] QueryOptions options)
    {
        Console.WriteLine("GetAllCategoriesAsync");
        try
        {
            return await _productImageService.GetAllProductImagesAsync(options);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ProductImageReadDTO> GetProductImageByIdAsync([FromRoute] Guid id)
    {
        return await _productImageService.GetProductImageById(id);
    }

    [HttpPost("/")]
    public async Task<ProductImageReadDTO> CreateProductImageByIdAsync([FromBody] ProductImageCreateDTO productImg)
    {
        return await _productImageService.CreateProductImage(productImg);
    }
    [HttpPatch("{id:guid}")]
    public async Task<ProductImageReadDTO> UpdateProductImageAsync([FromRoute] Guid id, [FromBody] ProductImageUpdateDTO category)
    {
        return await _productImageService.UpdateProductImage(id, category);
    }
    [HttpDelete("{id:guid}")]
    public async Task<bool> DeleteCategoryAsync([FromRoute] Guid id)
    {
        return await _productImageService.DeleteProductImage(id);
    }
}
