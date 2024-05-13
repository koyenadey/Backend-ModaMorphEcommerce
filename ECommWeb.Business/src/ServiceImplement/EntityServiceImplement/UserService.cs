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
        var addedUser = await _userRepo.CreateUserAsync(userToAdd);
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
        var addedUser = await _userRepo.CreateUserAsync(userToAdd);
        Console.WriteLine(addedUser.Id);
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
        //return users.Select(user => new UserReadDto().Transform(user));
    }

    public async Task<UserReadDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepo.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new ResourceNotFoundException("No user found by this id.");
        }
        if (user == null)
        {
            throw new ResourceNotFoundException("No user found by this id.");
        }
        return _mapper.Map<UserReadDto>(user);
        //return new UserReadDto().Transform(user);
    }

    public async Task<UserReadDto> UpdateUserByIdAsync(UserUpdateDto user)
    {
        var userToUpdate = await _userRepo.GetUserByIdAsync(user.Id);
        if (userToUpdate == null)
        {
            throw new ResourceNotFoundException("No user found to update.");
        }
        var userNewInfo = user.UpdateUser(userToUpdate);
        var updatedUser = await _userRepo.UpdateUserByIdAsync(userNewInfo);
        if (updatedUser == null)
        {
            throw new InvalidOperationException("Updating user failed.");
        }
        return _mapper.Map<UserReadDto>(updatedUser);
        //return new UserReadDto().Transform(updatedUser);
    }
    public async Task<bool> ChangePassword(Guid id, string password)
    {
        var userToUpdate = await GetUserByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new ResourceNotFoundException("No user found by this id.");
        }
        return await _userRepo.ChangePasswordAsync(id, password);
    }
}
