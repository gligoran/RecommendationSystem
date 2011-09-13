using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn.Bias.Models;

namespace RecommendationSystem.SvdBoostedKnn.Bias.Training
{
    public class BiasSvdBoostedKnnSvdTrainer : SvdTrainerBase<IBiasSvdBoostedKnnModel>
    {
        protected override IBiasSvdBoostedKnnModel GetNewModelInstance()
        {
            return new BiasSvdBoostedKnnModel();
        }
    }
}