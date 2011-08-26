using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Basic.Training
{
    public class BasicSvdTrainer : SvdTrainerBase<IBasicSvdModel>
    {
        public override IBasicSvdModel TrainModel(List<string> users, List<string> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var model = new BasicSvdModel();
            CalculateFeatures(model, trainingParameters);
            return model;
        }

        protected override float PredictRatingUsingResiduals(IBasicSvdModel model, int rating, int feature)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, Ratings[rating].UserIndex] * model.ArtistFeatures[feature, Ratings[rating].ArtistIndex];
        }
    }
}