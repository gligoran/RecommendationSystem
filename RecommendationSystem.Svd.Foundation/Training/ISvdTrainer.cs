using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public interface ISvdTrainer<TSvdModel> : ITrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        TSvdModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings, TrainingParameters trainingParameters);
        void SaveModel(string filename, TSvdModel model);
    }
}