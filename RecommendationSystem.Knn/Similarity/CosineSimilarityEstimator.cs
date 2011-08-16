using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommendationSystem.Knn.Similarity
{
    public class CosineSimilarityEstimator : ISimilarityEstimator
    {
        public float Similarity(User first, User second)
        {
            float rX, rY;
            float sumNum = 0.0f, sumX = 0.0f, sumY = 0.0f;

            int count = 0;
            foreach (var artist in first.Ratings.Keys)
            {
                if (second.Ratings.Keys.Contains(artist))
                {
                    count++;

                    rX = (float)first.Ratings[artist];
                    rY = (float)second.Ratings[artist];
                    sumNum += rX * rY;
                    sumX += (float)Math.Pow(rX, 2);
                    sumY += (float)Math.Pow(rY, 2);
                }
            }

            if (count == 0)
                return 0.0f;

            float mass = count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            return sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }
    }
}
