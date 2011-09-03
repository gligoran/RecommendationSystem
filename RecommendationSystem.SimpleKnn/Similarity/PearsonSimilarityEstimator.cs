using System;
using System.Linq;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Similarity
{
    public class PearsonSimilarityEstimator : ISimpleSimilarityEstimator
    {
        public float GetSimilarity(ISimpleKnnUser first, ISimpleKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var artistIndices = first.ArtistIndices.IntersectSorted(second.ArtistIndices).ToList();
            if (artistIndices.Count <= 0)
                return 0.0f;

            var rXavg = 0.0f;
            var rYavg = 0.0f;
            foreach (var artistIndex in artistIndices)
            {
                rXavg += first.RatingsByArtistIndexLookupTable[artistIndex].Value;
                rYavg += second.RatingsByArtistIndexLookupTable[artistIndex].Value;
            }

            rXavg /= artistIndices.Count;
            rYavg /= artistIndices.Count;

            foreach (var artistIndex in artistIndices)
            {
                var rX = first.RatingsByArtistIndexLookupTable[artistIndex].Value - rXavg;
                var rY = second.RatingsByArtistIndexLookupTable[artistIndex].Value - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var mass = artistIndices.Count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            var r = sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);

            if (float.IsNaN(r))
                return 0.0f;

            return Math.Abs(r);
        }

        public override string ToString()
        {
            return "PSE";
        }
    }
}