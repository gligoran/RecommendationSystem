using System;
using System.Linq;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class CosineSimilarityEstimator : ISimilarityEstimator
    {
        public float Similarity(User first, User second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var count = 0;
            foreach (var artist in first.Ratings.Keys.Where(artist => second.Ratings.Keys.Contains(artist)))
            {
                count++;

                var rX = first.Ratings[artist];
                var rY = second.Ratings[artist];
                sumNum += rX * rY;
                sumX += (float)Math.Pow(rX, 2);
                sumY += (float)Math.Pow(rY, 2);
            }

            if (count == 0)
                return 0.0f;

            var mass = count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            return sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }
    }
}