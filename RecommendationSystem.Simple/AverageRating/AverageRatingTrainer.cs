using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Training;

namespace RecommendationSystem.Simple.AverageRating
{
    public class AverageRatingTrainer : ITrainer<IAverageRatingModel, IUser>
    {
        public IAverageRatingModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            return new AverageRatingModel(ratings.Average(rating => rating.Value));
        }
    }
}