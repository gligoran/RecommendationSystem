using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.MatrixFactorization.Basic.Prediction;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Basic.Training
{
    public class BasicSvdTrainer : SvdTrainerBase<IBasicSvdModel>
    {
        public BasicSvdTrainer()
            : this(new BasicSvdPredictor())
        {}

        public BasicSvdTrainer(ISvdPredictor<IBasicSvdModel> predictor)
            : base(predictor)
        {}

        protected override IBasicSvdModel InitializeNewModel(List<string> users, List<string> artists, List<IRating> ratings)
        {
            return new BasicSvdModel();
        }

        protected override float PredictRatingUsingResiduals(IBasicSvdModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex];
        }
    }
}