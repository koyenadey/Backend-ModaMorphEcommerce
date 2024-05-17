using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productServices;

    public ProductController(IProductService productService)
    {
        _productServices = productService;
    }

    [HttpGet("api/v1/products")]
    public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProductsAsync([FromQuery] QueryOptions options)
    {
        var products = await _productServices.GetAllProductsAsync(options);
        if (products == null) return NotFound("Results could not be fetched");
        return Ok(products);
    }


    [HttpGet("api/v1/product/{id}")]
    public async Task<ActionResult<ProductReadDTO>> GetProductByIdAsync([FromRoute] Guid id)
    {
        var product = await _productServices.GetProductById(id);
        if (product == null) return NotFound("Product not found");
        return Ok(product);
    }


    [HttpGet("api/v1/products/category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProductsByCategoryAsync([FromRoute] Guid categoryId, [FromQuery] QueryOptions options)
    {
        var productsByCategory = await _productServices.GetAllProductsByCategoryAsync(categoryId, options);
        if (productsByCategory == null) return NotFound("Products not found");
        return Ok(productsByCategory);
    }


    [HttpGet("top/{topNumber:int}")]
    public async Task<IEnumerable<ProductReadDTO>> GetMostPurchased([FromRoute] int top)
    {
        return await _productServices.GetMostPurchasedProductsAsync(top);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("api/v1/product")]
    public async Task<ActionResult<ProductReadDTO>> CreateProductAsync([FromBody] ProductCreateDTO product)
    {
        var createdProduct = await _productServices.CreateProduct(product);
        if (createdProduct == null) return BadRequest("Product could not be created");
        return Ok(createdProduct);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("api/v1/product/{id}")]
    public async Task<ActionResult<ProductReadDTO>> UpdateProductAsync([FromRoute] Guid id, [FromBody] ProductUpdateDTO category)
    {
        var updatedProduct = await _productServices.UpdateProduct(id, category);
        if (updatedProduct == null) return BadRequest("Product could not be updated");
        return Ok(updatedProduct);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("api/v1/product/{id}")]
    public async Task<ActionResult<bool>> DeleteProductAsync([FromRoute] Guid id)
    {
        var isDeleted = await _productServices.DeleteProduct(id);
        if (!isDeleted) return BadRequest("Product could not be deleted");
        return Ok(isDeleted);
    }
}