using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public class MostCommonRatingRecommender : IRecommender<IMostCommonRatingModel>
    {
        public IPredictor<IMostCommonRatingModel> Predictor { get; set; }

        public MostCommonRatingRecommender()
        {
            Predictor = new MostCommonRatingPredictor();
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IMostCommonRatingModel model, List<IArtist> artists)
        {
            var indices = user.Ratings.Select(rating => rating.ArtistIndex).ToList();
            return indices.Select(index => new Recommendation(artists[index], model.MostCommonRating)).Cast<IRecommendation>().ToList();
        }

        public float PredictRatingForArtist(IUser user, IMostCommonRatingModel model, List<IArtist> artists, int artistIndex)
        {
            return Predictor.PredictRatingForArtist(user, model, artists, artistIndex);
        }
    }
}