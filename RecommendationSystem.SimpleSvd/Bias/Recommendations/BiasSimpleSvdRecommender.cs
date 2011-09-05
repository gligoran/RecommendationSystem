using RecommendationSystem.SimpleSvd.Bias.Prediction;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Bias.Recommendations
{
    public class BiasSimpleSvdRecommender : SimpleSvdRecommenderBase<IBiasSvdModel>
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