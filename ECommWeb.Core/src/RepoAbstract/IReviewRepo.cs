using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IReviewRepo
{
    public Task<IEnumerable<Review>> GetAllReviewsAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<Review>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId);
    public Task<Review> GetReviewByIdAsync(Guid reviewId);
    public Task<Review> CreateReviewAsync(Review review);
    public Task<Review> UpdateReviewByIdAsync(Guid reviewId, Review newReview);
    public Task<bool> DeleteReviewByIdAsync(Guid reviewId);
}
