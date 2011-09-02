using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.MatrixFactorization.Bias.Prediction;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.MatrixFactorization.Recommendation;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.MatrixFactorization.Bias.Recommendations
{
    public class BiasSvdRecommender : SvdRecommenderBase<IBiasSvdModel>
    {
        public BiasSvdRecommender(bool useBiasBins = false)
            : this(new BiasSvdPredictor(), useBiasBins)
        {}

        public BiasSvdRecommender(ISvdPredictor<IBiasSvdModel> predictor, bool useBiasBins = false)
            : base(predictor, useBiasBins)
        {}

        public override float PredictRatingForArtist(IUser user, IBiasSvdModel model, List<IArtist> artists, int artist)
        {
            return Predictor.PredictRatingForArtist(user, model, artists, artist, UseBiasBins);
        }

        public override IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IBiasSvdModel model, List<IArtist> artists)
        {
            throw new NotImplementedException();
        }
    }
}