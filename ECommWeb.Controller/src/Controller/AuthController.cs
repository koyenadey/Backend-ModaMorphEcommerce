using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;

namespace Server.Controller.src.Controller;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("api/v1/auth/login")]
    public async Task<ActionResult<AuthLoginReadDto>> LoginAsync([FromBody] UserCredential userCredential)
    {
        // return await _authService.LoginAsync(userCredential);
        var token = await _authService.Login(userCredential);
        if (token == null) return BadRequest("Could not generate token");
        return Ok(token);
    }

    [Authorize]
    [HttpGet("/api/v1/auth/profile")]
    public async Task<UserReadDto> GetUserProfile()
    {
        var userId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) throw new InvalidOperationException("Please login to use this facility!");

        var result = await _authService.GetCurrentProfile(Guid.Parse(userId));
        return _mapper.Map<UserReadDto>(result);
    }
}
