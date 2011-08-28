using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public interface ISvdTrainer<TSvdModel> : ITrainer<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {
        void SaveModel(string filename, TSvdModel model);
        TSvdModel LoadModel(string filename);
    }
}