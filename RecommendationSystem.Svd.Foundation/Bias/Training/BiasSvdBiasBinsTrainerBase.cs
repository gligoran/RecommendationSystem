using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Bias.Training
{
    public abstract class BiasSvdBiasBinsTrainerBase<TBiasSvdBiasBinsModel> : SvdBiasBinsTrainerBase<TBiasSvdBiasBinsModel>
        where TBiasSvdBiasBinsModel : IBiasSvdBiasBinsModel
    {
        #region Constructor
        protected BiasSvdBiasBinsTrainerBase(IBiasBinsCalculator<TBiasSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {
            ModelSaver.ModelPartSavers.Add(new BiasSvdModelPartSaver());
        }
        #endregion

        #region InitializeNewModel
        protected override void InitializeNewModel(TBiasSvdBiasBinsModel model, List<string> users, List<string> artists, List<IRating> ratings)
        {
            ComputeGlobalAverageAndBiases(model, users, artists, ratings);
        }
        #endregion

        #region PredictRatingUsingResiduals
        protected override float PredictRatingUsingResiduals(TBiasSvdBiasBinsModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] +
                   model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex] +
                   model.GlobalAverage +
                   model.UserBias[ratings[rating].UserIndex] +
                   model.ArtistBias[ratings[rating].ArtistIndex];
        }
        #endregion

        #region ComputeGlobalAverageAndBiases
        private void ComputeGlobalAverageAndBiases(TBiasSvdBiasBinsModel model, List<string> users, List<string> artists, List<IRating> ratings)
        {
            model.UserBias = new float[users.Count];
            model.ArtistBias = new float[artists.Count];

            model.GlobalAverage = ratings.Average(rating => rating.Value);

            var userCount = new int[users.Count];
            var artistCount = new int[artists.Count];
            foreach (var rating in ratings)
            {
                var d = rating.Value - model.GlobalAverage;

                model.UserBias[rating.UserIndex] += d;
                model.ArtistBias[rating.ArtistIndex] += d;

                userCount[rating.UserIndex] += 1;
                artistCount[rating.ArtistIndex] += 1;
            }

            for (var i = 0; i < model.UserBias.Length; i++)
                model.UserBias[i] /= userCount[i];

            for (var i = 0; i < model.ArtistBias.Length; i++)
            {
                if (artistCount[i] > 0)
                    model.ArtistBias[i] /= artistCount[i];
                else
                    model.ArtistBias[i] = 0.0f;
            }
        }
        #endregion
    }
}