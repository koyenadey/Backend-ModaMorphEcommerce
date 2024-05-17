using Moq;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;
using Xunit;
using AutoMapper;

namespace ECommWeb.Test.src.Service;

public class AddressServiceTests
{
    private readonly User user = SeedingData.GetUsers()[0];
    private readonly IMapper _mapper;
    private AddressService _addressService;
    public AddressServiceTests(IMapper mapper)
    {
        _mapper = mapper;
    }

    [Fact]
    public async Task CreateAddressAsync_ValidAddress_ReturnsAddressReadDto()
    {
        // Arrange
        var addressCreateDto = new AddressCreateDto
        {
            AddressLine = "41C",
            Street = "Asemakatu",
            City = "Pori",
            Country = "Finland",
            Postcode = "61200",
            PhoneNumber = "4198767000",
            Landmark = "K-market"
        };

        var addressToAdd = _mapper.Map<Address>(user);
        // addressCreateDto.CreateAddress(user.Id); // Mock or create an actual instance of Address
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.CreateAddressAsync(It.IsAny<Address>())).ReturnsAsync(addressToAdd);
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act
        var result = await _addressService.CreateAddressAsync(addressCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressToAdd.Postcode, result.Postcode);
    }


    [Fact]
    public async Task UpdateAddressByIdAsync_AddressNotFound_ThrowsResourceNotFoundException()
    {
        // Arrange
        var addressToUpdate = new Address
        {
            AddressLine = "41C",
            Street = "Asemakatu",
            City = "Pori",
            Country = "Finland",
            Postcode = "61200",
            PhoneNumber = "4198767000",
            Landmark = "K-market",
            UserId = user.Id
        };
        var addressId = addressToUpdate.Id;
        var addressUpdateDto = _mapper.Map<AddressUpdateDto>(addressToUpdate);
        //new AddressUpdateDto(addressId, "41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market");
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null);
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _addressService.UpdateAddressByIdAsync(addressId, addressUpdateDto));
    }

    [Fact]
    public async Task DeleteAddressByIdAsync_AddressNotFound_ThrowsResourceNotFoundException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(addressId)).ReturnsAsync(false); // Simulate delete failure
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _addressService.DeleteAddressByIdAsync(addressId));
    }

    [Fact]
    public async Task GetDefaultAddressAsync_UserHasNoDefaultAddress_ThrowsResourceNotFoundException()
    {
        // Arrange
        var mockAddressRepo = new Mock<IAddressRepo>();
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _addressService.GetDefaultAddressAsync(user.Id));
    }
    [Fact]
    public async Task GetDefaultAddressAsync_UserHasDefaultAddress_ReturnsAddressReadDto()
    {
        // Arrange
        var defaultAddress = new Address
        {
            AddressLine = "41C",
            Street = "Asemakatu",
            City = "Pori",
            Country = "Finland",
            Postcode = "61200",
            PhoneNumber = "4198767000",
            Landmark = "K-market",
            UserId = user.Id
        };
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(defaultAddress.Id)).ReturnsAsync(defaultAddress);

        user.DefaultAddressId = defaultAddress.Id; // Set default address ID for the user
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);
        Console.WriteLine(defaultAddress.Id);
        Console.WriteLine(user.DefaultAddressId);
        // Act
        var result = await _addressService.GetDefaultAddressAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(defaultAddress.Postcode, result.Postcode);
    }


    [Fact]
    public async Task SetDefaultAddressAsync_SettingDefaultAddressFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.SetDefaultAddressAsync(It.IsAny<Guid>(), addressId)).ReturnsAsync(false); // Simulate setting default address failure
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _addressService.SetDefaultAddressAsync(user.Id, addressId));
    }
    [Fact]
    public async Task SetDefaultAddressAsync_ValidAddressId_ReturnsTrue()
    {
        // Arrange
        var addressId = Guid.NewGuid(); // Provide a valid address ID
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.SetDefaultAddressAsync(It.IsAny<Guid>(), addressId)).ReturnsAsync(true); // Simulate successful setting of default address
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act
        var result = await _addressService.SetDefaultAddressAsync(user.Id, addressId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAddressByIdAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var addressId = Guid.NewGuid(); // Provide a valid address ID
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(addressId)).ReturnsAsync(true); // Simulate successful deletion
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act
        var result = await _addressService.DeleteAddressByIdAsync(addressId);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public async Task DeleteAddressByIdAsync_InvalidId_ThrowsResourceNotFoundException()
    {
        // Arrange
        var invalidAddressId = Guid.NewGuid(); // Provide an invalid address ID
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(invalidAddressId)).ReturnsAsync(false); // Simulate deletion failure
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _addressService.DeleteAddressByIdAsync(invalidAddressId));
    }
    [Fact]
    public async Task GetAddressByIdAsync_ValidId_ReturnsAddressReadDto()
    {
        // Arrange
        var addressId = Guid.NewGuid(); // Provide a valid address ID
        var address = new Address
        {
            AddressLine = "41C",
            Street = "Asemakatu",
            City = "Pori",
            Country = "Finland",
            Postcode = "61200",
            PhoneNumber = "4198767000",
            Landmark = "K-market",
            UserId = user.Id
        };
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync(address);
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act
        var result = await _addressService.GetAddressByIdAsync(addressId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(address.Postcode, result.Postcode);
    }
    [Fact]
    public async Task GetAddressByIdAsync_InvalidId_ThrowsResourceNotFoundException()
    {
        // Arrange
        var invalidAddressId = Guid.NewGuid(); // Provide an invalid address ID
        var mockAddressRepo = new Mock<IAddressRepo>();
        mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(invalidAddressId)).ReturnsAsync((Address)null); // Simulate address not found
        _addressService = new AddressService(mockAddressRepo.Object, _mapper);

        // Act + Assert
        await Assert.ThrowsAsync<ResourceNotFoundException>(() => _addressService.GetAddressByIdAsync(invalidAddressId));
    }


}
