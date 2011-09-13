using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public class KnnTrainerForSvdModels : KnnTrainerForSvdModelsBase<ISvdBoostedKnnModel>
    {
        public KnnTrainerForSvdModels()
            : this(new NewUserFeatureGenerator())
        {}

        public KnnTrainerForSvdModels(INewUserFeatureGenerator<ISvdBoostedKnnModel> newUserFeatureGenerator)
            : base(newUserFeatureGenerator)
        {}

        protected override ISvdBoostedKnnModel GetNewModelInstance()
        {
            return new SvdBoostedKnnModel();
        }
    }
}