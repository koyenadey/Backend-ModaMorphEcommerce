using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using ECommWeb.Business.src.Shared;


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
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsersAsync([FromQuery] QueryOptions options)
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
            var result = await _userService.GetAllUsersAsync(options);
            return Ok(result);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
            var results = await _userService.GetUserByIdAsync(id);
            return Ok(results);
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserReadDto>> GetUserProfileAsync()
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _userService.GetUserByIdAsync(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateCustomerAsync([FromBody] UserCreateDto user)
        {
            var createdUser = await _userService.CreateCustomerAsync(user);
            return CreatedAtAction("CreateCustomer", new { createdUser.Id }, createdUser);
        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound("No such user with the id provided exists!");

            var isDeleted = await _userService.DeleteUserByIdAsync(id);
            return Ok(isDeleted);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<UserReadDto> UpdateUserByIdAsync([FromBody] UserUpdateDto user, [FromRoute] Guid id)
        {
            var userClaims = (_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new InvalidOperationException("Please login to use this facility!");
            return await _userService.UpdateUserByIdAsync(id, user);
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