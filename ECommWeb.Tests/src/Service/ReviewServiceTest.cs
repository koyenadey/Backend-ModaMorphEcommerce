using Moq;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;
using ECommWeb.Test.src.Service.Data;
using Xunit;
using AutoMapper;
namespace ECommWeb.Test.src.Service.Data;

public class ReviewServiceTest
{
    private ReviewService _reviewService;
    private IMapper _mapper;
    Mock<IReviewRepo> _mockReviewRepo = new Mock<IReviewRepo>();
    public ReviewServiceTest(IMapper mapper)
    {
        _mapper = mapper;
    }


    [Fact]
    public async void CreateReviewAsync_CreatesAReview_ReturnsTheReview()
    {
        var userId = Guid.Parse("1972b1f3-bf6b-4f99-b46e-34abaf608ae9");
        var orderedProductId = Guid.Parse("ff05a8a2-b397-4833-aa80-f8291b0a4518");
        var review = new Review
        {
            Rating = 1.0,
            Comment = "Absolutely garbage.I dont recommend it at all",
            UserId = userId,
            OrderedProductId = orderedProductId
        };

        var createReviewDTO = new CreateReviewDTO
        {
            Rating = review.Rating,
            Comment = review.Comment,
            UserId = review.UserId,
            OrderedProductId = review.OrderedProductId
        };

        _mockReviewRepo.Setup(x => x.CreateReviewAsync(It.IsAny<Review>())).ReturnsAsync(review);
        _reviewService = new ReviewService(_mockReviewRepo.Object, _mapper);

        var createdReview = await _reviewService.CreateReviewAsync(createReviewDTO);

        Assert.Equal(review.Id, createdReview.Id);
    }

    [Fact]
    public async void GetAllReviewsByProductIdAsync_ReturnsAllReviews()
    {
        var productId = Guid.NewGuid();
        var options = new QueryOptions() { PageNo = 1, PageSize = 10 };

        List<Review> reviewsByProduct = new()
        {
            new Review
            {
                Rating = 1.0,
                Comment = "Waste of money",
                UserId = Guid.NewGuid(),
                OrderedProductId = Guid.NewGuid()
            },
            new Review
            {
                Rating = 4.9,
                Comment = "Great deal for the product",
                UserId = Guid.NewGuid(),
                OrderedProductId = Guid.NewGuid()
            }
        };

        _mockReviewRepo.Setup(x => x.GetAllReviewsByProductIdAsync(options, productId)).ReturnsAsync(reviewsByProduct);
        _reviewService = new ReviewService(_mockReviewRepo.Object, _mapper);

        var results = await _reviewService.GetAllReviewsByProductIdAsync(options, productId);

        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async void GetReviewByIdAsync_ThrowsException_ForInvalidId()
    {
        var review = new Review
        {
            Rating = 3.0,
            Comment = "Okayish product",
            UserId = Guid.NewGuid(),
            OrderedProductId = Guid.NewGuid()
        };
        _mockReviewRepo.Setup(x => x.GetReviewByIdAsync(Guid.NewGuid())).ReturnsAsync(review);

        _reviewService = new ReviewService(_mockReviewRepo.Object, _mapper);
        await Assert.ThrowsAsync<InvalidDataException>(async () => await _reviewService.GetReviewByIdAsync(Guid.NewGuid()));
    }

    [Theory]
    [ClassData(typeof(ReviewServiceTestData))]
    public void UpdateReviewByIdAsync_UpdatingWithValidValues_ReturnsUpdatedReview(UpdateReviewsDTO updatedReview)
    {
        var userId = Guid.NewGuid();
        var orderdProductId = Guid.NewGuid();

        var oldReview = new Review
        {
            Rating = 3.4,
            Comment = "Okayish product",
            UserId = userId,
            OrderedProductId = orderdProductId
        };

        oldReview.Rating = updatedReview.Rating;
        oldReview.Comment = updatedReview.Comment;

        //var newReview = updatedReview.UpdateReview(oldReview);


        _mockReviewRepo.Setup(x => x.GetReviewByIdAsync(oldReview.Id)).ReturnsAsync(oldReview);

        _mockReviewRepo.Setup(x => x.UpdateReviewByIdAsync(oldReview.Id, oldReview)).ReturnsAsync(oldReview);

        _reviewService = new ReviewService(_mockReviewRepo.Object, _mapper);

        var result = _reviewService.UpdateReviewByIdAsync(oldReview.Id, updatedReview);

        Assert.Equal(3.0, oldReview.Rating);
    }
}
