using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Training
{
    public interface IBiasBinsTrainer<TBiasBinsModel, TUser> : ITrainer<TBiasBinsModel, TUser>
        where TBiasBinsModel : IBiasBinsModel
        where TUser : IUser
    {
        IBiasBinsCalculator<TBiasBinsModel> BiasBinsCalculator { get; set; }
    }
}