using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo;

public class ReviewRepo : IReviewRepo
{
    private readonly AppDbContext _context;

    public ReviewRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Review> CreateReviewAsync(Review review)
    {
        var reviewImages = review.Images;
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        if (reviewImages is not null && reviewImages.Count() > 0)
        {
            foreach (var image in reviewImages)
            {
                _context.ReviewImages.Add(image);
            }
            await _context.SaveChangesAsync();
        }

        return review;
    }

    public async Task<bool> DeleteReviewByIdAsync(Guid reviewId)
    {
        bool isDeleted = false;
        Review? reviewFound = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (reviewFound != null)
        {
            _context.Reviews.Remove(reviewFound);
            await _context.SaveChangesAsync();
            isDeleted = true;
        }
        return isDeleted;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync(QueryOptions options, Guid userId)
    {
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;
        IQueryable<Review> query = _context.Reviews;
        var foundReviews = query
                            .OrderByDescending(r => r.ReviewDate)
                            .Skip((pgNo - 1) * pgSize)
                            .Take(pgSize);
        return await foundReviews.ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId)
    {
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;

        var orderedProductsIdList = await _context.OrderedProducts
                                    .Where(op => op.ProductId == productId)
                                    .Select(op => op.Id)
                                    .Skip((pgNo - 1) * pgSize)
                                    .Take(pgSize)
                                    .ToListAsync();

        var filteredOrderedReviews = await _context.Reviews
                            .Where(r => orderedProductsIdList.Contains(r.OrderedProductId))
                            .ToListAsync();

        return filteredOrderedReviews;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId)
    {
        return await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<Review> GetReviewByIdAsync(Guid reviewId)
    {
        var result = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        return result!;
    }

    public async Task<Review> UpdateReviewByIdAsync(Guid reviewId, Review newReview)
    {
        Review reviewFound = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
        if (reviewFound != null)
        {
            reviewFound.Comment = newReview.Comment;
            reviewFound.Rating = newReview.Rating;
            await _context.SaveChangesAsync();

        }
        return reviewFound!;
    }
}
