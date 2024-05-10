using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Core.src.Entity;

public class ReviewImage
{
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
    public string Image { get; set; }

    public ReviewImage()
    {

    }

    public ReviewImage(Guid reviewId, string imageurl) : this()
    {
        ReviewId = reviewId;
        Image = imageurl;
    }
}
