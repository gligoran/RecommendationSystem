using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommendationSystem.Knn.Similarity
{
    public class CosineSimilarityEstimator : ISimilarityEstimator
    {
        public double Similarity(User first, User second)
        {
            double rX, rY;
            double sumNum = 0.0, sumX = 0.0, sumY = 0.0;

            int count = 0;
            foreach (var artist in first.Ratings.Keys)
            {
                if (second.Ratings.Keys.Contains(artist))
                {
                    count++;

                    rX = (double)first.Ratings[artist];
                    rY = (double)second.Ratings[artist];
                    sumNum += rX * rY;
                    sumX += Math.Pow(rX, 2);
                    sumY += Math.Pow(rY, 2);
                }
            }

            if (count == 0)
                return 0;

            double mass = count * 2.0 / (first.Ratings.Count + second.Ratings.Count);
            return sumNum / (Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }
    }
}
