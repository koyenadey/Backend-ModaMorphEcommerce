using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommWeb.Core.src.Entity;
using Microsoft.AspNetCore.Http;

namespace ECommWeb.Business.src.DTO;

public class ReadReviewDTO
{
    public Guid Id { get; set; }
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public UserReadDto User { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.Now;
    public OrderProductReadDTO OrderedProduct { get; set; }
    public List<ReadReviewImageDTO>? Images { get; set; }
}

public class CreateReviewDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderedProductId { get; set; }
    public List<string>? Images { get; set; }
}

public class ReviewCreatePayloadDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; }
    public List<IFormFile>? Images { get; set; }
    public Guid UserId { get; set; }
    public Guid OrderedProductId { get; set; }

}

public class UpdateReviewsDTO
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}