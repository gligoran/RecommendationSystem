using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public abstract class SimilarityEstimatorBase : ISimilarityEstimator
    {
        public abstract float GetSimilarity(IUser first, IKnnUser second);

        protected List<Tuple<IRating, IRating>> GetRatingPairs(IUser first, IUser second)
        {
            var artists = first.Ratings.Select(rating => rating.ArtistIndex).Intersect(second.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            if (artists.Count == 0)
                return null;

            return (from artist in artists
                    let firstRating = first.Ratings.Where(rating => rating.ArtistIndex == artist).First()
                    let secondRating = second.Ratings.Where(rating => rating.ArtistIndex == artist).First()
                    select new Tuple<IRating, IRating>(firstRating, secondRating)).ToList();
        }
    }
}