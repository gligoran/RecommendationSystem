using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingRecommender : IRecommender<IAverageRatingModel>
    {
        public IPredictor<IAverageRatingModel> Predictor { get; set; }

        public AverageRatingRecommender()
        {
            Predictor = new AverageRatingPredictor();
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IAverageRatingModel model, List<IArtist> artists)
        {
            var indices = user.Ratings.Select(rating => rating.ArtistIndex).ToList();
            return indices.Select(index => new Recommendation(artists[index], model.AverageRating)).Cast<IRecommendation>().ToList();
        }

        public float PredictRatingForArtist(IUser user, IAverageRatingModel model, List<IArtist> artists, int artistIndex)
        {
            return Predictor.PredictRatingForArtist(user, model, artists, artistIndex);
        }
    }
}