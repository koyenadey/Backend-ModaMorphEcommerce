using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
[Route("api/v1/reviews")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReviewController(IReviewService reviewService, IHttpContextAccessor httpContextAccessor)
    {
        _reviewService = reviewService;
        _httpContextAccessor = httpContextAccessor;
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync([FromQuery] QueryOptions options)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var userId = Guid.Parse(userClaims);

        return await _reviewService.GetAllReviewsAsync(options, userId);
    }

    [Authorize]
    [HttpGet("admin")]
    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync([FromQuery] QueryOptions options)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var userId = Guid.Parse(userClaims);

        return await _reviewService.GetAllReviewsByUserAsync(options, userId);
    }

    [HttpGet("product/{id}")]
    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync([FromQuery] QueryOptions options, [FromRoute] Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentNullException("Product Id should be valid");
        return await _reviewService.GetAllReviewsByProductIdAsync(options, id);
    }

    [HttpGet("{id}")]
    public async Task<ReadReviewDTO> GetReviewByIdAsync([FromRoute] Guid id)
    {
        return await _reviewService.GetReviewByIdAsync(id);
    }

    [HttpPost]
    public async Task<ReadReviewDTO> CreateReviewAsync([FromBody] CreateReviewDTO review)
    {
        return await _reviewService.CreateReviewAsync(review);
    }

    [HttpPatch("{id}")]
    public async Task<ReadReviewDTO> UpdateReviewByIdAsync([FromRoute] Guid id, [FromBody] UpdateReviewsDTO updateReview)
    {
        return await _reviewService.UpdateReviewByIdAsync(id, updateReview);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteReviewByIdAsync([FromRoute] Guid id)
    {
        return await _reviewService.DeleteReviewByIdAsync(id);
    }
}