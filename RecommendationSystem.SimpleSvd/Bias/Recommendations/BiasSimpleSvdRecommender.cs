using RecommendationSystem.SimpleSvd.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.Svd.Foundation.Recommendations;

namespace RecommendationSystem.SimpleSvd.Bias.Recommendations
{
    public class BiasSimpleSvdRecommender : SvdRecommenderBase<IBiasSvdModel>
    {
        public BiasSimpleSvdRecommender()
            : this(new BiasSimpleSvdPredictor())
        {}

        public BiasSimpleSvdRecommender(ISvdPredictor<IBiasSvdModel> predictor)
            : base(predictor)
        {
            ModelLoader.ModelPartLoaders.Add(new BiasSvdModelPartLoader());
        }
    }
}