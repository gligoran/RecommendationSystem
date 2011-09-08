using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public class SvdBoostedKnnTrainer : SvdBoostedKnnTrainerBase<ISvdBoostedKnnModel>
    {
        public SvdBoostedKnnTrainer()
            : base(new NewUserFeatureGenerator())
        {}

        protected override ISvdBoostedKnnModel GetNewModelInstance()
        {
            return new SvdBoostedKnnModel();
        }
    }
}