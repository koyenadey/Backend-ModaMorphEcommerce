using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using AutoMapper;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addressRepo;
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    public AddressService(IAddressRepo addressRepo, IUserRepo userRepo, IMapper mapper)
    {
        _addressRepo = addressRepo;
        _userRepo = userRepo;
        _mapper = mapper;
    }
    public async Task<AddressReadDto> CreateAddressAsync(AddressCreateDto address)
    {
        var addressToAdd = _mapper.Map<Address>(address);
        var addedAddress = await _addressRepo.CreateAddressAsync(addressToAdd);
        if (addedAddress == null) throw new OperationFailedException("Unfortunately Address could not be created.");
        //return new AddressReadDto().MapToAddressReadDto(addedAddress);
        return _mapper.Map<AddressReadDto>(addedAddress);
    }

    public async Task<Guid> DeleteAddressByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ValidationException("Id is empty");

        var deletedId = await _addressRepo.DeleteAddressByIdAsync(id);

        if (deletedId == Guid.Empty) throw new ResourceNotFoundException("Address is not found.");

        return id;
    }

    public async Task<AddressReadDto> GetAddressByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ValidationException("Id is empty");

        var addressFound = await _addressRepo.GetAddressByIdAsync(id);

        if (addressFound == null) throw new ResourceNotFoundException("No Address found by this id.");

        return _mapper.Map<AddressReadDto>(addressFound);
    }

    public async Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync(Guid userId, QueryOptions? options)
    {
        if (userId == Guid.Empty) throw new ValidationException("Id is empty");

        var addresses = await _addressRepo.GetAddressesByUserAsync(userId, options);

        return _mapper.Map<IEnumerable<AddressReadDto>>(addresses);
    }


    public async Task<AddressReadDto> GetDefaultAddressAsync(Guid userId)
    {
        var defaultAddress = await _addressRepo.GetDefaultAddressAsync(userId);
        if (defaultAddress == null)
            throw new ResourceNotFoundException("The user doesn't have a default address.");

        //return new AddressReadDto().MapToAddressReadDto(defaultAddress);
        return _mapper.Map<AddressReadDto>(defaultAddress);
    }

    public async Task<AddressReadDto> SetDefaultAddressAsync(Guid userId, Guid addressId)
    {
        var defaultAddress = await _addressRepo.SetDefaultAddressAsync(userId, addressId);
        if (defaultAddress is null)
        {
            throw new InvalidOperationException("Setting default address failed. Please try again later.");
        }
        return _mapper.Map<AddressReadDto>(defaultAddress);
    }

    public async Task<AddressReadDto> UpdateAddressByIdAsync(Guid addressId, AddressUpdateDto addressDto)
    {
        var addressFound = await _addressRepo.GetAddressByIdAsync(addressId);

        if (addressFound == null)
            throw new ResourceNotFoundException("No address found to update.");

        var addressNewInfo = GetUpdatedAddress(addressFound, addressDto);

        var updatedAddress = await _addressRepo.UpdateAddressByIdAsync(addressNewInfo);

        if (updatedAddress == null)
            throw new InvalidOperationException("Updating address failed.");

        //return new AddressReadDto().MapToAddressReadDto(updatedAddress);
        return _mapper.Map<AddressReadDto>(updatedAddress);
    }
    private Address GetUpdatedAddress(Address addressToUpdate, AddressUpdateDto updateAddressDto)
    {
        addressToUpdate.AddressLine = updateAddressDto.AddressLine;
        addressToUpdate.City = updateAddressDto.City;
        addressToUpdate.Street = updateAddressDto.Street;
        addressToUpdate.Postcode = updateAddressDto.Postcode;
        addressToUpdate.PhoneNumber = updateAddressDto.PhoneNumber;
        addressToUpdate.Landmark = updateAddressDto.Landmark;

        return addressToUpdate;
    }
}
