using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Training
{
    public interface ITrainer<out TModel>
        where TModel : IModel
    {
        TModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings);
    }
}