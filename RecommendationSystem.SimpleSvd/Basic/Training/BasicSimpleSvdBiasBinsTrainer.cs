using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.SimpleSvd.Basic.Prediction;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleSvd.Basic.Training
{
    public class BasicSimpleSvdBiasBinsTrainer : SvdBiasBinsTrainerBase<IBasicSvdBiasBinsModel>
    {
        public BasicSimpleSvdBiasBinsTrainer()
            : this(new SvdBiasBinsCalculator<IBasicSvdBiasBinsModel>(new BasicSimpleSvdPredictor()))
        {}

        public BasicSimpleSvdBiasBinsTrainer(IBiasBinsCalculator<IBasicSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {}

        protected override IBasicSvdBiasBinsModel GetNewModelInstance(List<string> users, List<string> artists, List<IRating> ratings)
        {
            return new BasicSvdBiasBinsModel();
        }

        protected override float PredictRatingUsingResiduals(IBasicSvdBiasBinsModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex];
        }
    }
}