using RecommendationSystem.SimpleSvd.Basic.Prediction;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Basic.Recommendations
{
    public class BasicSimpleSvdRecommender : SimpleSvdRecommenderBase<IBasicSvdModel>
    {
        public BasicSimpleSvdRecommender()
            : this(new BasicSimpleSvdPredictor())
        {}

        public BasicSimpleSvdRecommender(ISvdPredictor<IBasicSvdModel> predictor)
            : base(predictor)
        {}
    }
}