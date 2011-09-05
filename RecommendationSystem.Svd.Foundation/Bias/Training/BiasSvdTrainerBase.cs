using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.Svd.Foundation.Bias.Training
{
    public abstract class BiasSvdTrainerBase<TBiasSvdModel> : SvdTrainerBase<TBiasSvdModel>
        where TBiasSvdModel : IBiasSvdModel
    {
        protected BiasSvdTrainerBase()
        {
            ModelSaver.ModelPartSavers.Add(new BiasSvdModelPartSaver());
        }
    }
}