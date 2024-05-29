using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract
{
    public interface IWishlistRepo
    {
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId);
        Task<Wishlist> DeleteWishlistByIdAsync(Guid id);
        Task<Wishlist> CreateWishlistAsync(Wishlist wishlist);
        Task<Wishlist> UpdateWishlistByIdAsync(Wishlist wishlist);
        Task<IEnumerable<Wishlist>> AddProductToWishlistAsync(Guid productId, Guid wishlistId);
        Task<WishlistItem> DeleteProductFromWishlistAsync(Guid productId, Guid wishlistId);
    }
}