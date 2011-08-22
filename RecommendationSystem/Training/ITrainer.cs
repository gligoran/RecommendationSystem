using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Training
{
    public interface ITrainer<out TModel, TUser>
        where TModel : IModel
        where TUser : IUser
    {
        TModel TrainModel(List<TUser> users, List<IArtist> artists, List<IRating> ratings);
    }
}