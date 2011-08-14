using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommendationSystem.Knn.Similarity
{
    public class PearsonSimilarityEstimator : ISimilarityEstimator
    {
        public double Similarity(User first, User second)
        {
            double rX, rY;
            double rXavg = 0.0, rYavg = 0.0;
            double sumNum = 0.0, sumX = 0.0, sumY = 0.0;

            var keys = new List<string>();
            foreach (var artist in first.Ratings.Keys)
            {
                if (second.Ratings.Keys.Contains(artist))
                {
                    keys.Add(artist);

                    rXavg += first.Ratings[artist];
                    rYavg += second.Ratings[artist];
                }
            }

            if (keys.Count == 0)
                return 0.0;

            rXavg /= keys.Count;
            rYavg /= keys.Count;

            foreach (var artist in keys)
            {
                rX = first.Ratings[artist] - rXavg;
                rY = second.Ratings[artist] - rYavg;

                sumNum += rX * rY;
                sumX += Math.Pow(rX, 2);
                sumY += Math.Pow(rY, 2);
            }

            double mass = keys.Count * 2.0 / (first.Ratings.Count + second.Ratings.Count);
            double r = sumNum / (Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);

            if (double.IsNaN(r))
                return 0.0;

            return Math.Abs(r);
        }
    }
}
