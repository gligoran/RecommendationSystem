using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.RatingAggregation;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations.RecommendationGeneration
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

        public float PredictRatingForArtist(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return RatingAggregator.Aggregate(simpleKnnUser, neighbours, artistIndex);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists)
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