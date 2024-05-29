using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Core.src.Entity;

public class ReviewImage : BaseEntity
{
    public Guid ReviewId { get; set; }
    public Review Review { get; set; }
    public string Image { get; set; }

}
