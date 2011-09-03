using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.RatingAggregation;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration
{
    public class RatingAggregationRecommendationGenerator<TModel, TKnnUser> : IRecommendationGenerator<TModel, TKnnUser>
        where TModel : IModel
        where TKnnUser : IKnnUser
    {
        public IRatingAggregator<TKnnUser> RatingAggregator { get; set; }

        public RatingAggregationRecommendationGenerator()
            : this(new SimpleAverageRatingAggregator<TKnnUser>())
        {}

        public RatingAggregationRecommendationGenerator(IRatingAggregator<TKnnUser> ratingAggregator)
        {
            RatingAggregator = ratingAggregator;
        }

        public float PredictRatingForArtist(TKnnUser simpleKnnUser, List<SimilarUser<TKnnUser>> neighbours, TModel model, List<IArtist> artists, int artistIndex)
        {
            return RatingAggregator.Aggregate(simpleKnnUser, neighbours, artistIndex);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(TKnnUser simpleKnnUser, List<SimilarUser<TKnnUser>> neighbours, TModel model, List<IArtist> artists)
        {
            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(simpleKnnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = (from artist in artistIndices
                                   let r = RatingAggregator.Aggregate(simpleKnnUser, neighbours, artist)
                                   where r > 0.0f
                                   select new Recommendation(artists[artist], r)).Cast<IRecommendation>().ToList();

            recommendations.Sort();
            return recommendations;
        }

        public override string ToString()
        {
            return RatingAggregator.ToString();
        }
    }
}