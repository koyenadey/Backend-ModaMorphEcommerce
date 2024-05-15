using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using AutoMapper;

namespace ECommWeb.Controller.src.Controller;


[ApiController]
[Route("api/v1/addresses")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;


    public AddressController(IAddressService addressService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _addressService = addressService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }


    [Authorize]
    [HttpGet] // define endpoint: /addresses?
    public async Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync([FromQuery] QueryOptions options)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null) throw new InvalidOperationException("Please login to use this facility!");

        var userId = Guid.Parse(userIdClaim);

        return await _addressService.GetAddressesByUserAsync(userId, options);
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<AddressReadDto> GetAddressByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");
        return await _addressService.GetAddressByIdAsync(id);
    }


    [Authorize]
    [HttpPost]
    public async Task<AddressReadDto> CreateAddressAsync([FromBody] AddressCreateDto address)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var result = await _addressService.CreateAddressAsync(address);
        return _mapper.Map<AddressReadDto>(result);
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<bool> DeleteAddressByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        return await _addressService.DeleteAddressByIdAsync(id);
    }


    [Authorize]
    [HttpPatch("{id}")]
    public async Task<AddressReadDto> UpdateAddressByIdAsync([FromBody] AddressUpdateDto address, [FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        return await _addressService.UpdateAddressByIdAsync(id, address);
    }


    [Authorize]
    [HttpPatch("{id}/set_default")]
    public async Task<bool> SetDefaultAddressAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");//unauthenticated
        var userId = Guid.Parse(userClaims);

        return await _addressService.SetDefaultAddressAsync(userId, id);
    }


    [Authorize]
    [HttpGet("default")]
    public async Task<AddressReadDto> GetDefaultAddressAsync()
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");//unauthenticated
        var userId = Guid.Parse(userClaims);
        return await _addressService.GetDefaultAddressAsync(userId);
    }
}