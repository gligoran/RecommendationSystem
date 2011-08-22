using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public interface ISvdTrainer<out TSvdModel> : ITrainer<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {}
}