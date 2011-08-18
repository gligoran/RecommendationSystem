using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.Knn.Similarity
{
    public class PearsonSimilarityEstimator : ISimilarityEstimator
    {
        public float Similarity(User.User first, User.User second)
        {
            float rXavg = 0.0f, rYavg = 0.0f;
            float sumNum = 0.0f, sumX = 0.0f, sumY = 0.0f;

            var keys = new List<string>();
            foreach (var artist in first.Ratings.Keys)
            {
                if (!second.Ratings.Keys.Contains(artist))
                    continue;

                keys.Add(artist);

                rXavg += first.Ratings[artist];
                rYavg += second.Ratings[artist];
            }

            if (keys.Count == 0)
                return 0.0f;

            rXavg /= keys.Count;
            rYavg /= keys.Count;

            foreach (var artist in keys)
            {
                var rX = first.Ratings[artist] - rXavg;
                var rY = second.Ratings[artist] - rYavg;

                sumNum += rX * rY;
                sumX += (float)Math.Pow(rX, 2);
                sumY += (float)Math.Pow(rY, 2);
            }

            float mass = keys.Count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            float r = sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);

            if (float.IsNaN(r))
                return 0.0f;

            return Math.Abs(r);
        }
    }
}
