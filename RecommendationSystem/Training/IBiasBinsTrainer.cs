using RecommendationSystem.Models;

namespace RecommendationSystem.Training
{
    public interface IBiasBinsTrainer<TBiasBinsModel> : ITrainer<TBiasBinsModel>
        where TBiasBinsModel : IBiasBinsModel
    {
        IBiasBinsCalculator<TBiasBinsModel> BiasBinsCalculator { get; set; }
    }
}