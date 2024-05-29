using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommWeb.Infrastructure.src.Repo;

public class WishlistRepo : IWishlistRepo
{
    private readonly AppDbContext _context;
    // private readonly User _user;
    public WishlistRepo(AppDbContext context)
    {
        _context = context;
        // _user = user;
    }
    public async Task<IEnumerable<Wishlist>> AddProductToWishlistAsync(Guid productId, Guid wishlistId)
    {
        await _context.WishlistItems.AddAsync(new WishlistItem
        {
            ProductId = productId,
            WishlistId = wishlistId
        });
        await _context.SaveChangesAsync();
        return await _context.Wishlists
                    .Where(wl => wl.Id == wishlistId)
                    .Include(wl => wl.User)
                    .Include(wl => wl.WishlistItems)
                        .ThenInclude(wli => wli.Product)
                            .ThenInclude(p => p.Images)
                    .Include(wl => wl.WishlistItems)
                        .ThenInclude(wli => wli.Product)
                            .ThenInclude(p => p.Category)
                    .ToListAsync();
    }

    public async Task<Wishlist> CreateWishlistAsync(Wishlist wishlist)
    {
        await _context.Wishlists.AddAsync(wishlist);
        await _context.SaveChangesAsync();
        return wishlist;
    }

    public async Task<WishlistItem> DeleteProductFromWishlistAsync(Guid productId, Guid wishlistId)
    {
        var wishlistItemToRemove = await _context.WishlistItems
                                        .Include(wl => wl.Product)
                                            .ThenInclude(p => p.Images)
                                        .Include(wl => wl.Product)
                                            .ThenInclude(p => p.Category)
                                        .FirstOrDefaultAsync(wli => wli.ProductId == productId && wli.WishlistId == wishlistId);
        if (wishlistItemToRemove != null)
        {
            _context.WishlistItems.Remove(wishlistItemToRemove);
            await _context.SaveChangesAsync();
            return wishlistItemToRemove;
        }
        else
        {
            return null;
        }
    }


    public async Task<Wishlist> DeleteWishlistByIdAsync(Guid id)
    {
        var wishlistToDelete = await GetWishlistByIdAsync(id);
        if (wishlistToDelete != null)
        {
            _context.Wishlists.Remove(wishlistToDelete);
            await _context.SaveChangesAsync();
            return wishlistToDelete; // Return true indicating successful deletion
        }
        else
        {
            return null; // Return false indicating user not found or deletion failed
        }
    }

    public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
    {
        var wishlist = await _context.Wishlists.FirstOrDefaultAsync(wl => wl.Id == id);
        return wishlist;
    }

    public async Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId)
    {
        return await _context.Wishlists
                    .Where(wl => wl.UserId == userId)
                    .Include(wl => wl.User)
                    .Include(wl => wl.WishlistItems)
                        .ThenInclude(wli => wli.Product)
                            .ThenInclude(p => p.Images)
                    .Include(wl => wl.WishlistItems)
                        .ThenInclude(wli => wli.Product)
                            .ThenInclude(p => p.Category)
                    .ToListAsync();
    }

    public async Task<Wishlist> UpdateWishlistByIdAsync(Wishlist wishlist)
    {
        _context.Wishlists.Update(wishlist);
        await _context.SaveChangesAsync();
        return wishlist;
    }
}
