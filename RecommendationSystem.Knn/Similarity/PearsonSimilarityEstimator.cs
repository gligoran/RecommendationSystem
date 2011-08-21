using System;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class PearsonSimilarityEstimator : SimilarityEstimatorBase
    {
        public override float GetSimilarity(IUser first, IKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var ratingPairs = GetRatingPairs(first, second);
            if (ratingPairs == null)
                return 0.0f;

            var rXavg = ratingPairs.Average(ratingPair => ratingPair.Item1.Value);
            var rYavg = ratingPairs.Average(ratingPair => ratingPair.Item2.Value);

            foreach (var ratingPair in ratingPairs)
            {
                var rX = ratingPair.Item1.Value - rXavg;
                var rY = ratingPair.Item2.Value - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var mass = ratingPairs.Count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            var r = sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);

            if (float.IsNaN(r))
                return 0.0f;

            return Math.Abs(r);
        }
    }
}