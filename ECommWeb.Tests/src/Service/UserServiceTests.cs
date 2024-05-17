using Moq;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;
using Xunit;
using AutoMapper;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using ECommWeb.Infrastructure.src;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Test.src.Service
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService passwordService = new PasswordService();
        public UserServiceTests(IMapper mapper)
        {
            _mapper = mapper;
        }
        [Fact]
        public async Task CreateCustomerAsync_EmailNotAvailable_ThrowsValidationException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);
            var userCreateDto = new Mock<UserCreateDto>("test", "test@example.com", "password");

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => userService.CreateCustomerAsync(userCreateDto.Object));
        }

        [Fact]
        public async Task CreateCustomerAsync_EmailAvailable_CreatesUser()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            var user = new User
            {
                UserName = "JohnMiller",
                Email = "john.miller@mail.com",
                Password = passwordService.HashPassword("miller123", out var salt),
                Salt = salt,
                Role = Role.Customer
            };
            var address = new Address
            {
                AddressLine = "41C",
                Street = "Asemakatu",
                City = "Pori",
                Country = "Finland",
                Postcode = "61200",
                PhoneNumber = "4198767000",
                Landmark = "K-market"
            };
            mockUserRepo.Setup(repo => repo.CreateUserAsync(user, address)).ReturnsAsync(user);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);
            var userCreateDto = new Mock<UserCreateDto>("JohnMiller", "john.miller@mail.com", "miller123");

            // Act
            var result = await userService.CreateCustomerAsync(userCreateDto.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john.miller@mail.com", result.Email);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_UserExists_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(true);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_UserNotExists_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(false);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.DeleteUserByIdAsync(userId));
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            var users = new List<User> {
                new User{
                    UserName = "miller",
                    Email = "miller@mail.com",
                    Password = passwordService.HashPassword("miller123", out var salt),
                    Salt = salt,
                    Role = Role.Customer
                },
                 new User{
                    UserName = "jane",
                    Email = "jane@testmail.com",
                    Password = passwordService.HashPassword("janetest1", out var salt1),
                    Salt = salt1,
                    Role = Role.Customer
                 },
                  new User{
                    UserName = "henry",
                    Email = "henry@testmail.com",
                    Password = passwordService.HashPassword("henrytest1", out var salt2),
                    Salt = salt2,
                    Role = Role.Customer
                  }
            };
            mockUserRepo.Setup(repo => repo.GetAllUsersAsync(It.IsAny<QueryOptions>())).ReturnsAsync(users);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.GetAllUsersAsync(new QueryOptions());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
        }
        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.GetUserByIdAsync(userId));
        }
        [Fact]
        public async Task GetUserByIdAsync_UserFound_ReturnsUserReadDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "miller",
                Email = "miller@mailc.om",
                Password = passwordService.HashPassword("miller123", out var salt),
                Salt = salt,
                Role = Role.Customer
            };
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDto
            {
                UserName = "henry",
                Password = passwordService.HashPassword("henrytest1", out var salt),
                Salt = salt,
            };
            // Create a user update DTO with the given ID
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.UpdateUserByIdAsync(userId, userUpdateDto));
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UpdateSuccessful_ReturnsUserReadDto()
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = Guid.NewGuid(),
                UserName = "miller",
                Email = "miller@mailc.om",
                Password = passwordService.HashPassword("miller123", out var salt),
                Salt = salt,
                Role = Role.Customer
            }; // Create a user to be updated with the given ID
            var userId = userToUpdate.Id;
            var userUpdateDto = new UserUpdateDto
            {
                UserName = "henry",
                Password = passwordService.HashPassword("henrytest1", out var salt1),
                Salt = salt1,
            }; // Create a user update DTO with the given ID

            userToUpdate.UserName = userUpdateDto.UserName;
            userToUpdate.Password = userUpdateDto.Password;
            userToUpdate.Salt = userUpdateDto.Salt;

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);

            var updatedUser = _mapper.Map<User>(userToUpdate);
            //userUpdateDto.UpdateUser(userToUpdate); // update the user 

            mockUserRepo.Setup(repo => repo.UpdateUserByIdAsync(updatedUser)).ReturnsAsync(updatedUser); // Simulate update success

            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.UpdateUserByIdAsync(userId, userUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userUpdateDto.UserName, result.UserName);
        }

        [Fact]
        public async Task ChangePassword_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null); // Simulate user not found
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.ChangePassword(userId, newPassword));
        }
        [Fact]
        public async Task ChangePassword_ChangeFailed_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var userToUpdate = new User
            {
                Id = Guid.NewGuid(),
                UserName = "miller",
                Email = "miller@mailc.om",
                Password = passwordService.HashPassword("miller123", out var salt),
                Salt = salt,
                Role = Role.Customer
            };
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, newPassword)).ReturnsAsync(false); // Simulate password change failure
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.ChangePassword(userId, newPassword);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task ChangePassword_ValidIdAndPassword_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var userToUpdate = new User
            {
                Id = Guid.NewGuid(),
                UserName = "miller",
                Email = "miller@mailc.om",
                Password = passwordService.HashPassword("miller123", out var salt),
                Salt = salt,
                Role = Role.Customer
            };
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, newPassword)).ReturnsAsync(true); // Simulate successful password change
            var userService = new UserService(mockUserRepo.Object, _mapper, passwordService);

            // Act
            var result = await userService.ChangePassword(userId, newPassword);

            // Assert
            // Verify that GetUserByIdAsync is invoked with the correct user ID
            mockUserRepo.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);

            // Verify that ChangePasswordAsync is invoked with the correct user ID and password
            mockUserRepo.Verify(repo => repo.ChangePasswordAsync(userId, newPassword), Times.Once);

            // Assert the result
            Assert.True(result);
        }


    }
}