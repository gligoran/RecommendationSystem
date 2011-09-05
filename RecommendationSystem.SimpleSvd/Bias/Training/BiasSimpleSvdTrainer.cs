using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Bias.Training;

namespace RecommendationSystem.SimpleSvd.Bias.Training
{
    public class BiasSimpleSvdTrainer : BiasSvdTrainerBase<IBiasSvdModel>
    {
        #region InitializeNewModel
        protected override IBiasSvdModel GetNewModelInstance()
        {
            return new BiasSvdModel();
        }
        #endregion
    }
}