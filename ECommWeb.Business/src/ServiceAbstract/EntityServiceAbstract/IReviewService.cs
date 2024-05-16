using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Business.src.DTO;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IReviewService
{
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId);
    public Task<ReadReviewDTO> GetReviewByIdAsync(Guid reviewId);
    public Task<ReadReviewDTO> CreateReviewAsync(CreateReviewDTO review);
    public Task<ReadReviewDTO> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO);
    public Task<bool> DeleteReviewByIdAsync(Guid reviewId);
}
