using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;


namespace Server.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync([FromQuery] QueryOptions options)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Please login to use this facility!");
            }

            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userClaims == null)
            {
                throw new InvalidOperationException("Invalid user claims.");
            }
            return await _userService.GetAllUsersAsync(options);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserReadDto> GetUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.GetUserByIdAsync(id);
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<UserReadDto> GetUserProfileAsync()
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _userService.GetUserByIdAsync(userId);
        }

        [HttpPost]
        public async Task<UserReadDto> CreateCustomerAsync([FromBody] UserCreateDto user)
        {
            return await _userService.CreateCustomerAsync(user);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.DeleteUserByIdAsync(id);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<UserReadDto> UpdateUserByIdAsync([FromBody] UserUpdateDto user)
        {
            var userClaims = (_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.UpdateUserByIdAsync(user);
        }
        [Authorize]
        [HttpPatch("change_password")]
        public async Task<bool> ChangePassword([FromBody] string newPassword)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var userId = Guid.Parse(userClaims);
            return await _userService.ChangePassword(userId, newPassword);
        }
    }
}