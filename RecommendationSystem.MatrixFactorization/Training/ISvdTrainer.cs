using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public interface ISvdTrainer<out TSvdModel> : ITrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        List<string> Users { get; set; }
        List<string> Artists { get; set; }
        List<IRating> Ratings { get; set; }
        TSvdModel TrainModel(TrainingParameters trainingParameters);
    }
}