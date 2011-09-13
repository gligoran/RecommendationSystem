using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public class SvdBoostedKnnSvdTrainer : SvdTrainerBase<ISvdBoostedKnnModel>
    {
        protected override ISvdBoostedKnnModel GetNewModelInstance()
        {
            return new SvdBoostedKnnModel();
        }
    }
}