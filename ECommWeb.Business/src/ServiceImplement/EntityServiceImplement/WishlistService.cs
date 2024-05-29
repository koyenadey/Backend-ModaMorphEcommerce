using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepo _wishlistRepo;
    private readonly IMapper _mapper;

    public WishlistService(IWishlistRepo wishlistRepo, IMapper mapper)
    {
        _wishlistRepo = wishlistRepo;
        _mapper = mapper;
    }
    public async Task<IEnumerable<WishlistReadDto>> AddProductToWishlistAsync(Guid productId, Guid wishlistId)
    {
        var wishlist = await _wishlistRepo.AddProductToWishlistAsync(productId, wishlistId);
        return _mapper.Map<IEnumerable<WishlistReadDto>>(wishlist);
    }

    public async Task<WishlistReadDto> CreateWishlistAsync(Guid userId, WishlistCreateDto wishlist)
    {
        // to do : duplicated wishlist name
        if (string.IsNullOrEmpty(wishlist.Name))
            throw new ArgumentNullException("Name cannot be empty!");

        var wishlistToAdd = _mapper.Map<Wishlist>(wishlist);
        var addedWishlist = await _wishlistRepo.CreateWishlistAsync(wishlistToAdd);
        return _mapper.Map<WishlistReadDto>(addedWishlist);
    }

    public async Task<WishlistReadItemDto> DeleteProductFromWishlistAsync(Guid productId, Guid wishlistId)
    {
        var wishlistItem = await _wishlistRepo.DeleteProductFromWishlistAsync(productId, wishlistId);
        return _mapper.Map<WishlistReadItemDto>(wishlistItem);
    }

    public async Task<bool> DeleteWishlistByIdAsync(Guid id)
    {
        var wishlist = await _wishlistRepo.DeleteWishlistByIdAsync(id);
        if (wishlist == null)
        {
            throw new ResourceNotFoundException("Wishlist is not found.");
        }
        return true;
    }

    public async Task<WishlistReadDto> GetWishlistByIdAsync(Guid id)
    {
        var wishlist = await _wishlistRepo.GetWishlistByIdAsync(id);
        if (wishlist == null)
        {
            throw new ResourceNotFoundException("No wishlist found by this id.");
        }
        return _mapper.Map<WishlistReadDto>(wishlist);
    }

    public async Task<IEnumerable<WishlistReadDto>> GetWishlistByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ValidationException("User id is empty.");

        var wishlists = await _wishlistRepo.GetWishlistByUserAsync(userId);
        return _mapper.Map<IEnumerable<WishlistReadDto>>(wishlists);
    }

    public async Task<WishlistReadDto> UpdateWishlistByIdAsync(WishlistUpdateDto wishlist, Guid wishlistId)
    {
        var wishlistToUpdate = await _wishlistRepo.GetWishlistByIdAsync(wishlistId);
        if (wishlistToUpdate == null)
        {
            throw new ResourceNotFoundException("No wishlist found to update.");
        }
        wishlistToUpdate.Name = wishlist.Name;
        var wishlistNewInfo = _mapper.Map<Wishlist>(wishlistToUpdate);

        var updatedWishlist = await _wishlistRepo.UpdateWishlistByIdAsync(wishlistNewInfo);
        if (updatedWishlist == null)
        {
            throw new InvalidOperationException("Updating wishlist failed.");
        }
        return _mapper.Map<WishlistReadDto>(updatedWishlist);
    }
}
