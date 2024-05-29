using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Core.src.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
[Route("/api/v1/reviews")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageUploadService _imageUploadService;

    public ReviewController(IReviewService reviewService, IHttpContextAccessor httpContextAccessor, IImageUploadService imageUploadService)
    {
        _reviewService = reviewService;
        _httpContextAccessor = httpContextAccessor;
        _imageUploadService = imageUploadService;
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
    public async Task<ReadReviewDTO> CreateReviewAsync([FromForm] ReviewCreatePayloadDTO review)
    {
        var reviewImageFiles = review.Images;
        if (reviewImageFiles is not null && reviewImageFiles.Any())
        {
            var reviewImages = await _imageUploadService.Upload(reviewImageFiles);
            var reviewDto = new CreateReviewDTO
            {
                Rating = review.Rating,
                Comment = review.Comment,
                Images = reviewImages,
                OrderedProductId = review.OrderedProductId,
                UserId = review.UserId
            };
            return await _reviewService.CreateReviewAsync(reviewDto);
        }
        var reviewDtoWoImage = new CreateReviewDTO
        {
            Rating = review.Rating,
            Comment = review.Comment,
            Images = null,
            OrderedProductId = review.OrderedProductId,
            UserId = review.UserId
        };
        return await _reviewService.CreateReviewAsync(reviewDtoWoImage);
    }

    [HttpPatch("{id}")]
    public async Task<ReadReviewDTO> UpdateReviewByIdAsync([FromRoute] Guid id, [FromBody] UpdateReviewsDTO updateReview)
    {
        return await _reviewService.UpdateReviewByIdAsync(id, updateReview);
    }

    [HttpDelete("{id}")]
    public async Task<ReadReviewDTO> DeleteReviewByIdAsync([FromRoute] Guid id)
    {
        return await _reviewService.DeleteReviewByIdAsync(id);
    }
}
