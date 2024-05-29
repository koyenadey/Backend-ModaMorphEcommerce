using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Common;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IReviewService
{
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId);
    public Task<ReadReviewDTO> GetReviewByIdAsync(Guid reviewId);
    public Task<ReadReviewDTO> CreateReviewAsync(CreateReviewDTO review);
    public Task<ReadReviewDTO> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO);
    public Task<ReadReviewDTO> DeleteReviewByIdAsync(Guid reviewId);
}
