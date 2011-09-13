using System;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Similarity
{
    public class CosineSvdBoostedKnnSimilarityEstimator : ISimilarityEstimator<ISvdBoostedKnnUser>
    {
        public float GetSimilarity(ISvdBoostedKnnUser first, ISvdBoostedKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            for (var i = 0; i < first.Features.Length; i++)
            {
                var rX = first.Features[i];
                var rY = second.Features[i];

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var r = sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY));

            return Math.Abs(r);
        }

        public override string ToString()
        {
            return "CSE";
        }
    }
}