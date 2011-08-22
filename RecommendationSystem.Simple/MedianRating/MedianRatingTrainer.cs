using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Simple.MedianRating
{
    public class MedianRatingTrainer : ITrainer<IMedianRatingModel, IUser>
    {
        public IMedianRatingModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            var ratingValues = ratings.Select(rating => rating.Value).OrderBy(value => value).ToList();
            var middle = ratingValues.Count / 2;
            return new MedianRatingModel(ratingValues[middle]);
        }
    }
}