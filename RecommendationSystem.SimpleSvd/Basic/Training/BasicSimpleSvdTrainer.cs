using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Basic.Training;

namespace RecommendationSystem.SimpleSvd.Basic.Training
{
    public class BasicSimpleSvdTrainer : BasicSvdTrainerBase<IBasicSvdModel>
    {
        protected override IBasicSvdModel GetNewModelInstance()
        {
            return new BasicSvdModel();
        }
    }
}