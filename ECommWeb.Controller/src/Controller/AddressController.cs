using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using ECommWeb.Core.src.ValueObject;

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
    [HttpGet("user/{id}")] // define endpoint: /addresses?
    public async Task<IEnumerable<AddressReadDto>> GetAddressesByUserIdAsync([FromRoute] Guid id, QueryOptions? queryOptions)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null) throw new InvalidOperationException("Please login to use this facility!");


        return await _addressService.GetAddressesByUserAsync(id, queryOptions);

    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AddressReadDto>> GetAddressByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) return BadRequest("Please login to use this facility!");
        if (id == Guid.Empty) return BadRequest("Please provide a valid id!");

        var loggedInUserId = Guid.Parse(userClaims);

        var address = await _addressService.GetAddressByIdAsync(id);

        if (address.User.Id != loggedInUserId) return Unauthorized("You are not authorized to access this address!");

        return Ok(address);
    }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AddressReadDto>> CreateAddressAsync([FromBody] AddressCreateDto address)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) return BadRequest("Please login to use this facility!");

        var result = await _addressService.CreateAddressAsync(address);
        return Ok(_mapper.Map<AddressReadDto>(result));
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAddressByIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var loggedInUserId = Guid.Parse(userClaims);
        var addressToBeDeleted = await _addressService.GetAddressByIdAsync(id);

        if (loggedInUserId != addressToBeDeleted.User.Id) return Unauthorized("You are not authorized to access this address!");

        return Ok(await _addressService.DeleteAddressByIdAsync(id));
    }


    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult<AddressReadDto>> UpdateAddressByIdAsync([FromBody] AddressUpdateDto address, [FromRoute] Guid id)
    {
        var userClaimsId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userClaimsRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        if (userClaimsId == null) return BadRequest("Please login to use this facility!");


        var requestedAddress = await _addressService.GetAddressByIdAsync(id);

        if (userClaimsRole != Role.Admin.ToString() && requestedAddress.User.Id != Guid.Parse(userClaimsId)) return Unauthorized("Sorry, this address does not belongs to you");


        return Ok(await _addressService.UpdateAddressByIdAsync(id, address));
    }


    [Authorize]
    [HttpPatch("{id}/setdefault")]
    public async Task<ActionResult<bool>> SetDefaultAddressAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");//unauthenticated

        var userId = Guid.Parse(userClaims);

        var addressRequested = await _addressService.GetAddressByIdAsync(id);

        if (addressRequested.User.Id != userId) return Unauthorized("Sorry this address does not belongs to you"); ;

        return Ok(await _addressService.SetDefaultAddressAsync(userId, id));
    }


    [Authorize]
    [HttpGet("default")]
    public async Task<ActionResult<AddressReadDto>> GetDefaultAddressAsync()
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) return BadRequest("Please login to use this facility!");//unauthenticated
        var userId = Guid.Parse(userClaims);
        return Ok(await _addressService.GetDefaultAddressAsync(userId));
    }

    [Authorize]
    [HttpGet("default/{id}")]
    public async Task<ActionResult<Guid>> GetDefaultAddressIdByUserIdAsync([FromRoute] Guid id)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaims == null) return BadRequest("Please login to use this facility!");//unauthenticated

        var defaultAddress = await _addressService.GetDefaultAddressAsync(id);
        if (defaultAddress == null) return NotFound("No default address found");
        return Ok(defaultAddress.Id);
    }
}