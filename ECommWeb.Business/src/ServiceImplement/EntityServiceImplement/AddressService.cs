using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addressRepo;
    public AddressService(IAddressRepo addressRepo)
    {
        _addressRepo = addressRepo;
    }
    public async Task<AddressReadDto> CreateAddressAsync(Guid userId, AddressCreateDto address)
    {
        var addressToAdd = address.CreateAddress(userId);
        var addedAddress = await _addressRepo.CreateAddressAsync(addressToAdd);
        return new AddressReadDto().Transform(addedAddress);
    }

    public async Task<bool> DeleteAddressByIdAsync(Guid id)
    {
        var isDeleted = await _addressRepo.DeleteAddressByIdAsync(id);
        if (!isDeleted)
        {
            throw new ResourceNotFoundException("Address is not found.");
        }
        return true;
    }

    public async Task<AddressReadDto> GetAddressByIdAsync(Guid id)
    {
        var address = await _addressRepo.GetAddressByIdAsync(id);
        if (address == null)
        {
            throw new ResourceNotFoundException("No Address found by this id.");
        }
        return new AddressReadDto().Transform(address);
    }

    public async Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync(Guid userId, QueryOptions? options)
    {
        var addresses = await _addressRepo.GetAddressesByUserAsync(userId, options);
        return addresses.Select(a => new AddressReadDto().Transform(a));
    }


    public async Task<AddressReadDto> GetDefaultAddressAsync(Guid userId)
    {
        var defaultAddress = await _addressRepo.GetDefaultAddressAsync(userId);
        if (defaultAddress == null)
            throw new ResourceNotFoundException("The user doesn't have a default address.");
        return new AddressReadDto().Transform(defaultAddress);
    }

    public async Task<bool> SetDefaultAddressAsync(Guid userId, Guid addressId)
    {
        var isSet = await _addressRepo.SetDefaultAddressAsync(userId, addressId);
        if (!isSet)
        {
            throw new InvalidOperationException("Setting default address failed. Please try again later.");
        }
        return isSet;
    }

    public async Task<AddressReadDto> UpdateAddressByIdAsync(AddressUpdateDto address)
    {
        var addressToUpdate = await _addressRepo.GetAddressByIdAsync(address.Id);
        if (addressToUpdate == null)
        {
            throw new ResourceNotFoundException("No address found to update.");
        }
        var addressNewInfo = address.UpdateAddress(addressToUpdate);
        var updatedAddress = await _addressRepo.UpdateAddressByIdAsync(addressNewInfo);
        if (updatedAddress == null)
        {
            throw new InvalidOperationException("Updating address failed.");
        }
        return new AddressReadDto().Transform(updatedAddress);
    }
}
