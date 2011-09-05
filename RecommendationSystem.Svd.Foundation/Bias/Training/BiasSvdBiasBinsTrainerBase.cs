using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Bias.Training
{
    public abstract class BiasSvdBiasBinsTrainerBase : SvdBiasBinsTrainerBase<IBiasSvdBiasBinsModel>
    {
        protected BiasSvdBiasBinsTrainerBase(IBiasBinsCalculator<IBiasSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {
            ModelSaver.ModelPartSavers.Add(new BiasSvdModelPartSaver());
        }
    }
}