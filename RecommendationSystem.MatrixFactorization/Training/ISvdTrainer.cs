using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public interface ISvdTrainer<TSvdModel> : ITrainer<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {
        ISvdPredictor<TSvdModel> Predictor { get; set; }
        TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters);
    }
}