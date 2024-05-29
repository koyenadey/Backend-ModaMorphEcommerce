using System.Security.Claims;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
[Route("/api/v1/wishlists")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public WishlistController(IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor)
    {
        _wishlistService = wishlistService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<WishlistReadDto>> GetWishlistByUsersAsync()
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var userId = Guid.Parse(userClaims);
        return await _wishlistService.GetWishlistByUserAsync(userId);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<WishlistReadDto> GetWishlistByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        return await _wishlistService.GetWishlistByIdAsync(id);
    }
    [Authorize]
    [HttpPost]
    public async Task<WishlistReadDto> CreateWishlistAsync([FromBody] WishlistCreateDto wishlist)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var userId = Guid.Parse(userClaims);
        return await _wishlistService.CreateWishlistAsync(userId, wishlist);
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<bool> DeleteWishlistByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        return await _wishlistService.DeleteWishlistByIdAsync(id);
    }
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<WishlistReadDto> UpdateWishlistByIdAsync([FromBody] WishlistUpdateDto wishlist, [FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        return await _wishlistService.UpdateWishlistByIdAsync(wishlist, id);
    }
    [Authorize]
    [HttpPost("{id}/add_product")]
    public async Task<IEnumerable<WishlistReadDto>> AddProductToWishlishAsync([FromBody] AddToWishlistDto addToWishlistDto, [FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var productId = addToWishlistDto.ProductId;
        return await _wishlistService.AddProductToWishlistAsync(productId, id);
    }
    [Authorize]
    [HttpDelete("{id}/delete_product")]
    public async Task<WishlistReadItemDto> DeleteProductFromWishlishAsync([FromBody] AddToWishlistDto wishlistItemToDel, [FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        var productId = wishlistItemToDel.ProductId;
        return await _wishlistService.DeleteProductFromWishlistAsync(productId, id);
    }
}
