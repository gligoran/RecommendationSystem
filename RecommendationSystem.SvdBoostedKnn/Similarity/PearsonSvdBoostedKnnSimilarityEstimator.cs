using System;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Similarity
{
    public class PearsonSvdBoostedKnnSimilarityEstimator<TSvdBoostedKnnUser> : ISvdBoostedKnnSimilarityEstimator<TSvdBoostedKnnUser>
        where TSvdBoostedKnnUser : ISvdBoostedKnnUser
    {
        public float GetSimilarity(TSvdBoostedKnnUser first, TSvdBoostedKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var rXavg = 0.0f;
            var rYavg = 0.0f;
            for (var i = 0; i < first.Features.Length; i++)
            {
                rXavg += first.Features[i];
                rYavg += second.Features[i];
            }
            rXavg /= first.Features.Length;
            rYavg /= second.Features.Length;

            for (var i = 0; i < first.Features.Length; i++)
            {
                var rX = first.Features[i] - rXavg;
                var rY = second.Features[i] - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var r = sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY));

            return Math.Abs(r);
        }

        public override string ToString()
        {
            return "PSE";
        }
    }
}