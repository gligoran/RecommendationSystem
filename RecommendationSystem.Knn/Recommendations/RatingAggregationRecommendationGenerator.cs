using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public class RatingAggregationRecommendationGenerator : IRecommendationGenerator
    {
        public IRatingAggregator RatingAggregator { get; set; }

        public RatingAggregationRecommendationGenerator()
            : this(new SimpleAverageRatingAggregator())
        {}

        public RatingAggregationRecommendationGenerator(IRatingAggregator ratingAggregator)
        {
            RatingAggregator = ratingAggregator;
        }

        public float PredictRatingForArtist(IKnnUser knnUser, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return RatingAggregator.Aggregate(knnUser, neighbours, artistIndex);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IKnnUser knnUser, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists)
        {
            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(knnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = (from artist in artistIndices
                                   let r = RatingAggregator.Aggregate(knnUser, neighbours, artist)
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