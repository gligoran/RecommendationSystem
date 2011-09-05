using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.Svd.Foundation.Basic.Training
{
    public abstract class BasicSvdTrainerBase<TBasicSvdModel> : SvdTrainerBase<TBasicSvdModel>
        where TBasicSvdModel : IBasicSvdModel
    {
        protected override float PredictRatingUsingResiduals(TBasicSvdModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex];
        }
    }
}