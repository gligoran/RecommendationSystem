using RecommendationSystem.Prediction;
using RecommendationSystem.SimpleSvd.Basic.Prediction;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Basic.Models;

namespace RecommendationSystem.SimpleSvd.Basic.Recommendations
{
    public class BasicSimpleSvdBiasBinsRecommender : SimpleSvdBiasBinsRecommenderBase<IBasicSvdBiasBinsModel>
    {
        public BasicSimpleSvdBiasBinsRecommender()
            : this(new BasicSimpleSvdPredictor())
        {}

        public BasicSimpleSvdBiasBinsRecommender(IPredictor<IBasicSvdBiasBinsModel> predictor)
            : this(predictor, new BiasBinsAdjustor<IBasicSvdBiasBinsModel>())
        {}

        public BasicSimpleSvdBiasBinsRecommender(IPredictor<IBasicSvdBiasBinsModel> predictor, IBiasBinsAdjustor<IBasicSvdBiasBinsModel> biasBinsAdjustor)
            : base(predictor, biasBinsAdjustor)
        {}
    }
}