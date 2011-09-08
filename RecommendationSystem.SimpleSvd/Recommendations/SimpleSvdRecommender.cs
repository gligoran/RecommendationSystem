using RecommendationSystem.SimpleSvd.Prediction;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.Svd.Foundation.Recommendations;

namespace RecommendationSystem.SimpleSvd.Recommendations
{
    public class SimpleSvdRecommender : SvdRecommenderBase<ISvdModel>
    {
        public SimpleSvdRecommender()
            : this(new SimpleSvdPredictor())
        {}

        public SimpleSvdRecommender(ISvdPredictor<ISvdModel> predictor)
            : base(predictor)
        {}
    }
}