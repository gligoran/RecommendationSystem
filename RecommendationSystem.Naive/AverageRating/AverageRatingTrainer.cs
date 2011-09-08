using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingTrainer : ITrainer<IAverageRatingModel>
    {
        public IAverageRatingModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings)
        {
            return new AverageRatingModel(trainRatings.Average(rating => rating.Value));
        }
    }
}