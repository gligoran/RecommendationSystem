using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Simple.MedianRating
{
    public class MedianRatingRecommender : IRecommender<IMedianRatingModel>
    {
        public List<IRecommendation> GenerateRecommendations(IUser user, IMedianRatingModel model, List<IArtist> artists)
        {
            var indices = user.Ratings.Select(rating => rating.ArtistIndex).ToList();
            return indices.Select(index => new Recommendation(artists[index], model.MedianRating)).Cast<IRecommendation>().ToList();
        }
    }
}