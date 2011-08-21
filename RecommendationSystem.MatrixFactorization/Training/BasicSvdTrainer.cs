using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public class BasicSvdTrainer : SvdTrainerBase<ISvdModel>
    {
        public BasicSvdTrainer(List<IRating> ratings, List<string> users, List<string> artists)
            : base(ratings, users, artists)
        {}

        public override ISvdModel TrainModel(TrainingParameters trainingParameters)
        {
            var model = new BasicSvdModel();
            CalculateFeatures(model, trainingParameters);
            return model;
        }

        protected override float PredictRatingUsingResiduals(ISvdModel model, int rating, int feature)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, Ratings[rating].UserIndex] * model.ArtistFeatures[feature, Ratings[rating].ArtistIndex];
        }
    }
}