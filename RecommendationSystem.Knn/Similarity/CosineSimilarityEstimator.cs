using System;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class CosineSimilarityEstimator : ISimilarityEstimator
    {
        public float GetSimilarity(IKnnUser first, IKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var artistIndices = first.ArtistIndices.IntersectSorted(second.ArtistIndices).ToList();
            if (artistIndices.Count <= 0)
                return 0.0f;

            foreach (var artistIndex in artistIndices)
            {
                var rX = first.RatingsByArtistIndexLookupTable[artistIndex].Value;
                var rY = second.RatingsByArtistIndexLookupTable[artistIndex].Value;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var mass = artistIndices.Count * 2.0f / (first.Ratings.Count + second.Ratings.Count);
            return sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }

        public override string ToString()
        {
            return "CSE";
        }
    }
}