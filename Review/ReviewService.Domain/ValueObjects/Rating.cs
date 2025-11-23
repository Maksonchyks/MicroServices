using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;

namespace ReviewService.Domain.ValueObjects
{
    public class Rating : ValueObject
    {
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public Rating(int score, int maxScore = 5) 
        {
            if (score < 0 || score > maxScore)
                throw new InvalidRatingException($"Score must be beetwen 0 and {maxScore}");

            if (maxScore < 0)
                throw new InvalidRatingException("MaxScore must be greater than 0");

            Score = score;
            MaxScore = maxScore;
        }

        public double GetPercentage() =>(double)Score / MaxScore * 100;

        public static Rating Empty => new Rating(0);
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Score;
            yield return MaxScore;
        }

    }
    public class InvalidRatingException : DomainException
    {
        public InvalidRatingException(string message) : base(message) { }
    }
}
