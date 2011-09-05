using RecommendationSystem.Prediction;
using RecommendationSystem.SimpleSvd.Bias.Prediction;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Bias.Models;

namespace RecommendationSystem.SimpleSvd.Bias.Recommendations
{
    public class BiasSimpleSvdBiasBinsRecommender : SimpleSvdBiasBinsRecommenderBase<IBiasSvdBiasBinsModel>
    {
        public BiasSimpleSvdBiasBinsRecommender()
            : this(new BiasSimpleSvdPredictor())
        {}

        public BiasSimpleSvdBiasBinsRecommender(IPredictor<IBiasSvdBiasBinsModel> predictor)
            : this(predictor, new BiasBinsAdjustor<IBiasSvdBiasBinsModel>())
        {}

        public BiasSimpleSvdBiasBinsRecommender(IPredictor<IBiasSvdBiasBinsModel> predictor, IBiasBinsAdjustor<IBiasSvdBiasBinsModel> biasBinsAdjustor)
            : base(predictor, biasBinsAdjustor)
        {
            ModelLoader.ModelPartLoaders.Add(new BiasSvdModelPartLoader());
        }
    }
}