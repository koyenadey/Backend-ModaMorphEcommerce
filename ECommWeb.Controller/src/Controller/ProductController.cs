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
    public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync([FromQuery] QueryOptions options)
    {
        Console.WriteLine("GetAllProductsAsync");
        try
        {
            return await _productServices.GetAllProductsAsync(options);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    [HttpGet("api/v1/product/{id}")]
    public async Task<ProductReadDTO> GetProductByIdAsync([FromRoute] Guid id)
    {
        return await _productServices.GetProductById(id);
    }


    [HttpGet("api/v1/products/category/{categoryId}")]
    public async Task<IEnumerable<ProductReadDTO>> GetAllProductsByCategoryAsync([FromRoute] Guid categoryId, [FromQuery] QueryOptions options)
    {
        Console.WriteLine("GetAllProductsByCategoryAsync");
        try
        {
            return await _productServices.GetAllProductsByCategoryAsync(categoryId, options);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    [HttpGet("top/{topNumber:int}")]
    public async Task<IEnumerable<ProductReadDTO>> GetMostPurchased([FromRoute] int top)
    {
        return await _productServices.GetMostPurchasedProductsAsync(top);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("api/v1/product")]
    public async Task<ProductReadDTO> CreateProductAsync([FromBody] ProductCreateDTO product)
    {
        return await _productServices.CreateProduct(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("api/v1/product/{id}")]
    public async Task<ProductReadDTO> UpdateProductAsync([FromRoute] Guid id, [FromBody] ProductUpdateDTO category)
    {
        return await _productServices.UpdateProduct(id, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("api/v1/product/{id}")]
    public async Task<bool> DeleteProductAsync([FromRoute] Guid id)
    {
        return await _productServices.DeleteProduct(id);
    }
}