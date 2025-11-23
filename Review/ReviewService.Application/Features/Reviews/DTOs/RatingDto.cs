using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Features.Reviews.DTOs
{
    public class RatingDto
    {
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public double Percentage => (double)Score / MaxScore * 100;
    }
}
