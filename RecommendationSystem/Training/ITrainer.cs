using RecommendationSystem.Models;

namespace RecommendationSystem.Training
{
    public interface ITrainer<out TModel>
        where TModel : IModel
    {
        TModel TrainModel();
    }
}