using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd.Basic.Training
{
    public class BasicSimpleSvdTrainer : SvdTrainerBase<IBasicSvdModel>
    {
        protected override IBasicSvdModel GetNewModelInstance(List<string> users, List<string> artists, List<IRating> ratings)
        {
            return new BasicSvdModel();
        }

        protected override float PredictRatingUsingResiduals(IBasicSvdModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] + model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex];
        }
    }
}