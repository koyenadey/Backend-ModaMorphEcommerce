using ECommWeb.Business.src.DTO;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IWishlistService
{
    Task<WishlistReadDto> GetWishlistByIdAsync(Guid id);
    Task<IEnumerable<WishlistReadDto>> GetWishlistByUserAsync(Guid userId);
    Task<bool> DeleteWishlistByIdAsync(Guid id);
    Task<WishlistReadDto> CreateWishlistAsync(Guid userId, WishlistCreateDto wishlist);
    Task<WishlistReadDto> UpdateWishlistByIdAsync(WishlistUpdateDto wishlist, Guid wishlistId);
    Task<IEnumerable<WishlistReadDto>> AddProductToWishlistAsync(Guid productId, Guid wishlistId);
    Task<WishlistReadItemDto> DeleteProductFromWishlistAsync(Guid productId, Guid wishlistId);
}
