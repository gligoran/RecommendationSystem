using RecommendationSystem.Prediction;
using RecommendationSystem.SimpleSvd.Prediction;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Recommendations;

namespace RecommendationSystem.SimpleSvd.Recommendations
{
    public class SimpleSvdBiasBinsRecommender : SvdBiasBinsRecommenderBase<ISvdBiasBinsModel>
    {
        public SimpleSvdBiasBinsRecommender()
            : this(new SimpleSvdPredictor())
        {}

        public SimpleSvdBiasBinsRecommender(IPredictor<ISvdBiasBinsModel> predictor)
            : this(predictor, new BiasBinsAdjustor<ISvdBiasBinsModel>())
        {}

        public SimpleSvdBiasBinsRecommender(IPredictor<ISvdBiasBinsModel> predictor, IBiasBinsAdjustor<ISvdBiasBinsModel> biasBinsAdjustor)
            : base(predictor, biasBinsAdjustor)
        {}
    }
}