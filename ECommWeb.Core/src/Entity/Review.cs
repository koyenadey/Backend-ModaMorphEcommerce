using System.ComponentModel.DataAnnotations;

namespace ECommWeb.Core.src.Entity;

public class Review : BaseEntity
{
    public Guid Id { get; set; }

    [Range(1.0, 5.0,
        ErrorMessage = "Value for {0} must be between {1} and {2}")]
    public double Rating { get; set; }

    [MinLength(5)]
    public string Comment { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime ReviewDate { get; set; }
    public Guid OrderedProductId { get; set; }
    //public OrderProducts OrderedProduct { get; set; }


    //public IEnumerable<ReviewImages> Images { get; set; }

    public Review(double rating, string comment, Guid userId, Guid orderedProductId)
    {
        Id = Guid.NewGuid();
        Rating = rating;
        Comment = comment;
        UserId = userId;
        ReviewDate = DateTime.Now;
        OrderedProductId = orderedProductId;
    }
}
