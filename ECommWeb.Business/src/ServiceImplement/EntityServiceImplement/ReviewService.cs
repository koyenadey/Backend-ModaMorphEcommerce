using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using AutoMapper;
using ECommWeb.Core.src.Entity;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement;

public class ReviewService : IReviewService
{
    private readonly IReviewRepo _reviewrepo;
    private readonly IMapper _mapper;

    public ReviewService(IReviewRepo reviewrepo, IMapper mapper)
    {
        _reviewrepo = reviewrepo;
        _mapper = mapper;
    }
    public async Task<ReadReviewDTO> CreateReviewAsync(CreateReviewDTO review)
    {
        if (review == null) throw new ValidationException("Review cannot be null");

        var reviewToAdd = _mapper.Map<Review>(review);
        var createdReview = await _reviewrepo.CreateReviewAsync(reviewToAdd);

        return _mapper.Map<ReadReviewDTO>(createdReview);
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ValidationException("User cannot be null");

        var reviews = await _reviewrepo.GetAllReviewsAsync(options, userId);

        return _mapper.Map<IEnumerable<ReadReviewDTO>>(reviews);
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId)
    {
        if (productId == Guid.Empty) throw new ValidationException("Product Id cannot be empty");

        var reviews = await _reviewrepo.GetAllReviewsByProductIdAsync(options, productId);

        return _mapper.Map<IEnumerable<ReadReviewDTO>>(reviews);
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ValidationException("User Id should be a valid input");

        var reviews = await _reviewrepo.GetAllReviewsByUserAsync(options, userId);

        return _mapper.Map<IEnumerable<ReadReviewDTO>>(reviews);
    }

    public async Task<ReadReviewDTO> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ValidationException("Review Id cannot be empty");

        var review = await _reviewrepo.GetReviewByIdAsync(reviewId);

        if (review == null) throw new ResourceNotFoundException("The review Id provided is incorrect");

        return _mapper.Map<ReadReviewDTO>(review);
    }

    public async Task<ReadReviewDTO> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO)
    {
        if (reviewId == Guid.Empty) throw new ValidationException("Review Id cannot be empty");

        if (updateReviewsDTO == null) throw new ValidationException("Review cannot be null");

        var review = await _reviewrepo.GetReviewByIdAsync(reviewId);

        if (review == null) throw new ResourceNotFoundException("The review Id provided is incorrect");

        review.Rating = updateReviewsDTO.Rating == default ? review.Rating : updateReviewsDTO.Rating;
        review.Comment = string.IsNullOrWhiteSpace(updateReviewsDTO.Comment) ? review.Comment : updateReviewsDTO.Comment;

        var updatedReview = await _reviewrepo.UpdateReviewByIdAsync(reviewId, review);

        if (updatedReview == null) throw new OperationFailedException("The update could not be done!");

        return _mapper.Map<ReadReviewDTO>(updatedReview);
    }

    public async Task<bool> DeleteReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ValidationException("Review Id cannot be empty");

        var isDeleted = await _reviewrepo.DeleteReviewByIdAsync(reviewId);

        if (!isDeleted) throw new OperationFailedException("The review could not be deleted!");

        return isDeleted;
    }
}
