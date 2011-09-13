using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Bias.Models;
using RecommendationSystem.SvdBoostedKnn.Training;

namespace RecommendationSystem.SvdBoostedKnn.Bias.Training
{
    public class BiasKnnTrainerForSvdModels : KnnTrainerForSvdModelsBase<IBiasSvdBoostedKnnModel>
    {
        public BiasKnnTrainerForSvdModels()
            : this(new BiasNewUserFeatureGenerator())
        {}

        public BiasKnnTrainerForSvdModels(INewUserFeatureGenerator<IBiasSvdBoostedKnnModel> newUserFeatureGenerator)
            : base(newUserFeatureGenerator)
        {
            ModelLoader.ModelPartLoaders.Add(new BiasSvdModelPartLoader());
        }

        protected override IBiasSvdBoostedKnnModel GetNewModelInstance()
        {
            return new BiasSvdBoostedKnnModel();
        }
    }
}