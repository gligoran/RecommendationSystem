using RecommendationSystem.SimpleSvd.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Bias.Training;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleSvd.Bias.Training
{
    public class BiasSimpleSvdBiasBinsTrainer : BiasSvdBiasBinsTrainerBase<IBiasSvdBiasBinsModel>
    {
        #region Constructor
        public BiasSimpleSvdBiasBinsTrainer()
            : this(new SvdBiasBinsCalculator<IBiasSvdBiasBinsModel>(new BiasSimpleSvdPredictor()))
        {}

        public BiasSimpleSvdBiasBinsTrainer(IBiasBinsCalculator<IBiasSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {}
        #endregion

        #region GetNewModelInstance
        protected override IBiasSvdBiasBinsModel GetNewModelInstance()
        {
            return new BiasSvdBiasBinsModel();
        }
        #endregion
    }
}