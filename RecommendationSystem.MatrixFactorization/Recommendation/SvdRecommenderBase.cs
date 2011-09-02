using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.MatrixFactorization.Recommendation
{
    public abstract class SvdRecommenderBase<TSvdModel> : ISvdRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public ISvdPredictor<TSvdModel> Predictor { get; set; }
        public bool UseBiasBins { get; set; }

        protected SvdRecommenderBase(ISvdPredictor<TSvdModel> predictor, bool useBiasBins = false)
        {
            Predictor = predictor;
            UseBiasBins = useBiasBins;
        }

        public abstract float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex);
        public abstract IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TSvdModel model, List<IArtist> artists);
    }
}