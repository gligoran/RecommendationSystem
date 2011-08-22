using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Simple.AverageRating
{
    public class AverageRatingRecommender : IRecommender<IAverageRatingModel>
    {
        public List<IRecommendation> GenerateRecommendations(IUser user, IAverageRatingModel model, List<IArtist> artists)
        {
            var indices = user.Ratings.Select(rating => rating.ArtistIndex).ToList();
            return indices.Select(index => new Recommendation(artists[index], model.AverageRating)).Cast<IRecommendation>().ToList();
        }
    }
}