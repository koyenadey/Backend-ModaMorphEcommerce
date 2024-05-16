using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class ReadReviewDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserName { get; set; }
    public string ProductName { get; set; }
    public DateTime ReviewDate { get; set; }
    public IEnumerable<ReviewImage> ReviewImages { get; set; }

}

public class CreateReviewDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid OrderedProductId { get; set; }
    public IEnumerable<ReviewImage>? ReviewImages { get; set; }

}

public class UpdateReviewsDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

}
