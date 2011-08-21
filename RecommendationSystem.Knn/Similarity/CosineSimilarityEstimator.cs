using System;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class CosineSimilarityEstimator : SimilarityEstimatorBase
    {
        public override float GetSimilarity(IUser first, IKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var ratingPairs = GetRatingPairs(first, second);
            if (ratingPairs == null)
                return 0.0f;

            foreach (var ratingPair in ratingPairs)
            {
                var rX = ratingPair.Item1.Value;
                var rY = ratingPair.Item2.Value;
                sumNum += rX * rY;
                sumX += (float)Math.Pow(rX, 2);
                sumY += (float)Math.Pow(rY, 2);
            }

            var mass = ratingPairs.Count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            return sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }
    }
}