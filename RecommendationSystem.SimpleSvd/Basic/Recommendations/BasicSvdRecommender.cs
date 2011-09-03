using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleSvd.Basic.Prediction;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Basic.Recommendations
{
    public class BasicSvdRecommender : SvdRecommenderBase<IBasicSvdModel>
    {
        public BasicSvdRecommender(bool useBiasBins = false)
            : this(new BasicSvdPredictor(), useBiasBins)
        {}

        public BasicSvdRecommender(ISvdPredictor<IBasicSvdModel> predictor, bool useBiasBins = false)
            : base(predictor, useBiasBins)
        {}

        public override float PredictRatingForArtist(IUser user, IBasicSvdModel model, List<IArtist> artists, int artist)
        {
            return Predictor.PredictRatingForArtist(user, model, artists, artist, UseBiasBins);
        }

        public override IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IBasicSvdModel model, List<IArtist> artists)
        {
            throw new NotImplementedException();
        }
    }
}