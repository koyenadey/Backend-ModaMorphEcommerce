using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Business.src.Shared;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

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
        if (createdReview == null) throw CustomException.NotFoundException("Review could not be created");
        return _mapper.Map<ReadReviewDTO>(createdReview);
        //return new ReadReviewDTO().ReadReviews(createdReview);
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User cannot be null");
        var reviews = await _reviewrepo.GetAllReviewsAsync(options, userId);
        if (reviews is null) throw CustomException.NotFoundException("Review not found");
        return _mapper.Map<IEnumerable<ReadReviewDTO>>(reviews);
        //return reviews.Select(r => new ReadReviewDTO().ReadReviews(r)).ToList();
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Product Id cannot be empty");
        var reviews = await _reviewrepo.GetAllReviewsByProductIdAsync(options, productId);
        if (reviews is null) throw CustomException.NotFoundException("Review not found");
        return _mapper.Map<IEnumerable<ReadReviewDTO>>(reviews);
        //return reviews.Select(r => new ReadReviewDTO().ReadReviews(r));
    }

    public async Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id should be a valid input");
        var results = await _reviewrepo.GetAllReviewsByUserAsync(options, userId);
        if (results is null) throw CustomException.NotFoundException("Review not found");
        return _mapper.Map<IEnumerable<ReadReviewDTO>>(results);
        //return results.Select(r => new ReadReviewDTO().ReadReviews(r));
    }

    public async Task<ReadReviewDTO> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ArgumentNullException("Review Id cannot be empty");
        var result = await _reviewrepo.GetReviewByIdAsync(reviewId);

        if (result == null) throw new InvalidDataException("The review Id provided is incorrect");
        return _mapper.Map<ReadReviewDTO>(result);
        //return new ReadReviewDTO().ReadReviews(result);
    }

    public async Task<ReadReviewDTO> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO)
    {
        if (reviewId == Guid.Empty) throw new ArgumentNullException("Review Id cannot be empty");
        if (updateReviewsDTO == null) throw new ArgumentNullException("Review cannot be null");

        var foundReview = await _reviewrepo.GetReviewByIdAsync(reviewId);

        if (foundReview == null) throw new InvalidDataException("The review Id provided is incorrect");

        foundReview.Comment = updateReviewsDTO.Comment;
        foundReview.Rating = updateReviewsDTO.Rating;

        var result = await _reviewrepo.UpdateReviewByIdAsync(reviewId, foundReview);
        if (result is null) throw CustomException.NotFoundException("Review not found");
        return _mapper.Map<ReadReviewDTO>(result);

    }

    public async Task<ReadReviewDTO> DeleteReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ValidationException("Review Id cannot be empty");
        var result = await _reviewrepo.DeleteReviewByIdAsync(reviewId);
        if (result is null) throw new OperationFailedException("Sorry...The deletion was not successful!");
        return _mapper.Map<ReadReviewDTO>(result);
    }
}
