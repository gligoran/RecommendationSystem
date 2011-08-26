using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Simple.MostCommonRating
{
    public class MostCommonRatingRecommender : IRecommender<IMostCommonRatingModel>
    {
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IMostCommonRatingModel model, List<IArtist> artists)
        {
            var indices = user.Ratings.Select(rating => rating.ArtistIndex).ToList();
            return indices.Select(index => new Recommendation(artists[index], model.MostCommonRating)).Cast<IRecommendation>().ToList();
        }

        public float PredictRatingForArtist(IUser user, IMostCommonRatingModel model, List<IArtist> artists, int artist)
        {
            return model.MostCommonRating;
        }
    }
}