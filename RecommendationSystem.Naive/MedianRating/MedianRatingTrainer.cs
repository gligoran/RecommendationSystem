using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MedianRating
{
    public class MedianRatingTrainer : ITrainer<IMedianRatingModel>
    {
        public IMedianRatingModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings)
        {
            var ratingValues = trainRatings.Select(rating => rating.Value).OrderBy(value => value).ToList();
            var middle = ratingValues.Count / 2;
            return new MedianRatingModel(ratingValues[middle]);
        }
    }
}