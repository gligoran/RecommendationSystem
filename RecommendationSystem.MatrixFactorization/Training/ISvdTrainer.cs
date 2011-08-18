using System.Collections.Generic;
using RecommendationSystem.Data;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public interface ISvdTrainer<out TSvdModel>
        where TSvdModel : ISvdModel
    {
        List<string> Users { get; set; }
        List<string> Artists { get; set; }
        List<Rating> Ratings { get; set; }
        TSvdModel TrainModel(TrainingParameters trainingParameters);
    }
}