using System;
using System.Linq;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Similarity
{
    public class UnionPearsonSimilarityEstimator : ISimpleSimilarityEstimator
    {
        public float GetSimilarity(ISimpleKnnUser first, ISimpleKnnUser second)
        {
            var artistIndices = first.ArtistIndices.IntersectSorted(second.ArtistIndices).ToList();

            var rXavg = first.Ratings.Average(r => r.Value);
            var rYavg = second.Ratings.Average(r => r.Value);

            float sumNum = 0.0f,
                  sumX = 0.0f,
                  sumY = 0.0f;

            foreach (var artistIndex in artistIndices)
            {
                var rX = first.RatingsByArtistIndexLookupTable[artistIndex].Value - rXavg;
                var rY = second.RatingsByArtistIndexLookupTable[artistIndex].Value - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var firstIndices = first.ArtistIndices.Except(artistIndices).ToList();
            foreach (var artistIndex in firstIndices)
            {
                var rX = first.RatingsByArtistIndexLookupTable[artistIndex].Value - rXavg;
                var rY = 1.0f - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            var secondIndices = second.ArtistIndices.Except(artistIndices).ToList();
            foreach (var artistIndex in secondIndices)
            {
                var rX = 1.0f - rXavg;
                var rY = second.RatingsByArtistIndexLookupTable[artistIndex].Value - rYavg;

                sumNum += rX * rY;
                sumX += rX * rX;
                sumY += rY * rY;
            }

            return Math.Abs(sumNum / (float)(Math.Sqrt(sumX) * Math.Sqrt(sumY)));
        }

        public override string ToString()
        {
            return "uPSE";
        }
    }
}