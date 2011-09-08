using RecommendationSystem.Prediction;
using RecommendationSystem.SimpleSvd.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Recommendations;

namespace RecommendationSystem.SimpleSvd.Bias.Recommendations
{
    public class BiasSimpleSvdBiasBinsRecommender : SvdBiasBinsRecommenderBase<IBiasSvdBiasBinsModel>
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