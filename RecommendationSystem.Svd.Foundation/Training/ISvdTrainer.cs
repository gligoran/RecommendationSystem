using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public interface ISvdTrainer<TSvdModel> : ITrainer<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {
        TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters);
        void SaveModel(string filename, TSvdModel model);
    }
}