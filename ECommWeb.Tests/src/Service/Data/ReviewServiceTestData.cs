using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommWeb.Business.src.DTO;
using Xunit;

namespace ECommWeb.Test.src.Service.Data;

public class ReviewServiceTestData : TheoryData<UpdateReviewsDTO>
{
    public ReviewServiceTestData()
    {
        var updateReviewDto = new UpdateReviewsDTO
        {
            Rating = 3.0,
            Comment = "Okayish product"
        };

        Add(updateReviewDto);
    }
}
