using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class ReadReviewDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.Now;


    public ReadReviewDTO ReadReviews(Review review)
    {
        return new ReadReviewDTO()
        {
            Id = review.Id,
            Rating = review.Rating,
            Comment = review.Comment,
            UserId = review.UserId,
        };
    }
}

public class CreateReviewDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderedProductId { get; set; }
    public ReviewImage[]? ReviewImages { get; set; }
    public CreateReviewDTO(double rating, string comment, Guid userId, Guid orderdProductId, ReviewImage[] reviewsImage)
    {
        Rating = rating;
        Comment = comment;
        UserId = userId;
        OrderedProductId = orderdProductId;
        ReviewImages = reviewsImage;

    }
    public Review CreateReviews()
    {
        return new Review(Rating, Comment, UserId, OrderedProductId);
    }
}

public class UpdateReviewsDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public UpdateReviewsDTO(double rating, string comment)
    {
        Rating = rating;
        Comment = comment;
    }

    public Review UpdateReview(Review oldreview)
    {
        if (oldreview.Rating != 0.0 && !String.IsNullOrEmpty(oldreview.Comment))
        {
            oldreview.Rating = Rating;
            oldreview.Comment = Comment;
        }

        return oldreview;
    }
}
