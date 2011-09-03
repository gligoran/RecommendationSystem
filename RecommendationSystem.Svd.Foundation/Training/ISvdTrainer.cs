using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public interface ISvdTrainer<TSvdModel> : ITrainer<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {
        ISvdPredictor<TSvdModel> Predictor { get; set; }
        TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters);
    }
}