using System;
using System.Linq;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class UnionCosineSimilarityEstimator : ISimilarityEstimator
    {
        public float GetSimilarity(IKnnUser first, IKnnUser second)
        {
            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            var artistIndices = first.ArtistIndices.Union(second.ArtistIndices).ToList();

            foreach (var artistIndex in artistIndices)
            {
                var rX = 1.0f;
                if (first.RatingsByArtistIndexLookupTable.ContainsKey(artistIndex))
                    rX = first.RatingsByArtistIndexLookupTable[artistIndex].Value;

                var rY = 1.0f;
                if (second.RatingsByArtistIndexLookupTable.ContainsKey(artistIndex))
                    rY = second.RatingsByArtistIndexLookupTable[artistIndex].Value;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            return sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY));
        }

        public override string ToString()
        {
            return "uCSE";
        }
    }
}