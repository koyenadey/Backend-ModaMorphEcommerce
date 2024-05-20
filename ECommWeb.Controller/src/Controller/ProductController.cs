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
    private readonly IImageUploadService _imageUploadService;

    public ProductController(IProductService productService, IImageUploadService imageUploadService)
    {
        _productServices = productService;
        _imageUploadService = imageUploadService;
    }

    [HttpGet("api/v1/products/meta")]
    public async Task<ActionResult<int>> GetProductsCount()
    {
        var productCount = await _productServices.GetProductsCount();

        if (productCount == 0) return NotFound("Results could not be fetched");

        return Ok(productCount);
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
    public async Task<ActionResult<ProductReadDTO>> CreateProductAsync([FromForm] ProductCreatePayloadDTO productPayload)
    {

        var files = productPayload.Images;

        // Upload images (if any)
        var imageUrls = await _imageUploadService.Upload(files);

        if (imageUrls == null) return BadRequest("Images could not be uploaded");

        var product = new ProductCreateDTO
        {
            Name = productPayload.Name,
            Description = productPayload.Description,
            Price = productPayload.Price,
            Inventory = productPayload.Inventory,
            CategoryId = productPayload.CategoryId,
            Images = imageUrls
        };

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