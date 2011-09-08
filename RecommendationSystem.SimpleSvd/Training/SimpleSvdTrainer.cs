using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd.Training
{
    public class SimpleSvdTrainer : SvdTrainerBase<ISvdModel>
    {
        protected override ISvdModel GetNewModelInstance()
        {
            return new SvdModel();
        }
    }
}