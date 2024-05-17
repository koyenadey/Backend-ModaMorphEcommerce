using AutoMapper;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Core.src.Entity;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepo userRepo, IMapper mapper, IPasswordService passwordService)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    public async Task<bool> CheckEmailAsync(string email)
    {
        return await _userRepo.CheckEmailAsync(email);
    }

    public async Task<UserReadDto> CreateAdminAsync(UserCreateDto user)
    {
        var userToAdd = _mapper.Map<User>(user);
        userToAdd.Password = _passwordService.HashPassword(userToAdd.Password, out byte[] salt);
        userToAdd.Salt = salt;
        userToAdd.Role = Role.Admin;
        var adminAddress = _mapper.Map<Address>(GetAddress(user, userToAdd.Id));
        var addedUser = await _userRepo.CreateUserAsync(userToAdd, adminAddress);
        return _mapper.Map<UserReadDto>(addedUser);
    }


    public async Task<UserReadDto> CreateCustomerAsync(UserCreateDto user)
    {
        var isEmailRegistered = await _userRepo.CheckEmailAsync(user.Email);
        if (isEmailRegistered)
        {
            throw new ValidationException("Email has been registered. Maybe try to login...");
        }
        var hashedPassword = _passwordService.HashPassword(user.Password, out byte[] salt);

        var userToAdd = _mapper.Map<User>(user);
        userToAdd.Password = hashedPassword;
        userToAdd.Salt = salt;

        var userAddress = _mapper.Map<Address>(GetAddress(user, userToAdd.Id));

        userToAdd.DefaultAddressId = userAddress.Id;

        var addedUser = await _userRepo.CreateUserAsync(userToAdd, userAddress);

        return _mapper.Map<UserReadDto>(addedUser);

    }

    public async Task<bool> DeleteUserByIdAsync(Guid id)
    {
        var isDeleted = await _userRepo.DeleteUserByIdAsync(id);
        if (!isDeleted)
        {
            throw new ResourceNotFoundException("User is not found.");
        }
        return true;
    }

    public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(QueryOptions options)
    {
        var users = await _userRepo.GetAllUsersAsync(options);
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<UserReadDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepo.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new ResourceNotFoundException("No user found by this id.");
        }
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<UserReadDto> UpdateUserByIdAsync(Guid userId, UserUpdateDto user)
    {
        var userToUpdate = await _userRepo.GetUserByIdAsync(userId);

        if (userToUpdate == null) throw new ResourceNotFoundException("No user found to update.");
        if (user.UserName == null) throw new ValidationException("User name is required.");
        if (user.Password == null) throw new ValidationException("Password is required.");

        userToUpdate.UserName = user.UserName;
        userToUpdate.Password = _passwordService.HashPassword(user.Password, out byte[] salt);
        userToUpdate.Salt = salt;

        var updatedUser = await _userRepo.UpdateUserByIdAsync(userToUpdate);
        if (updatedUser == null)
        {
            throw new InvalidOperationException("Updating user failed.");
        }

        return _mapper.Map<UserReadDto>(updatedUser);
    }
    public async Task<bool> ChangePassword(Guid id, string password)
    {
        var userToUpdate = await GetUserByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new ResourceNotFoundException("No user found by this id.");
        }
        password = _passwordService.HashPassword(password, out byte[] salt);
        return await _userRepo.ChangePasswordAsync(id, password, salt);
    }

    private AddressCreateDto GetAddress(UserCreateDto userCreateDto, Guid userId)
    {
        var userAddress = new AddressCreateDto
        {
            UserId = userId,
            AddressLine = userCreateDto.AddresLine1,
            Street = userCreateDto.Street,
            City = userCreateDto.City,
            Country = userCreateDto.Country,
            Postcode = userCreateDto.Postcode,
            Landmark = userCreateDto.Landmark,
            PhoneNumber = userCreateDto.PhoneNumber,
        };
        return userAddress;
    }

}
