using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Basic.Training
{
    public abstract class BasicSvdBiasBinsTrainerBase<TBasicSvdBiasBinsModel> : SvdBiasBinsTrainerBase<TBasicSvdBiasBinsModel>
        where TBasicSvdBiasBinsModel : IBasicSvdBiasBinsModel
    {
        protected BasicSvdBiasBinsTrainerBase(IBiasBinsCalculator<TBasicSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {}

        protected override float PredictRatingUsingResiduals(TBasicSvdBiasBinsModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex];
        }
    }
}