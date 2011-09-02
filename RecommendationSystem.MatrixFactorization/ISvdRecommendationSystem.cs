using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Recommendation;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization
{
    public interface ISvdRecommendationSystem<TSvdModel> : IRecommendationSystem<TSvdModel, IUser, ISvdTrainer<TSvdModel>, ISvdRecommender<TSvdModel>>
        where TSvdModel : ISvdModel
    {
        void SaveModel(string filename, TSvdModel model);
        TSvdModel LoadModel(string filename);
    }
}