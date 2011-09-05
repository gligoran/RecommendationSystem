using RecommendationSystem.SimpleSvd.Basic.Prediction;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Basic.Training;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleSvd.Basic.Training
{
    public class BasicSimpleSvdBiasBinsTrainer : BasicSvdBiasBinsTrainerBase<IBasicSvdBiasBinsModel>
    {
        public BasicSimpleSvdBiasBinsTrainer()
            : this(new SvdBiasBinsCalculator<IBasicSvdBiasBinsModel>(new BasicSimpleSvdPredictor()))
        {}

        public BasicSimpleSvdBiasBinsTrainer(IBiasBinsCalculator<IBasicSvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {}

        protected override IBasicSvdBiasBinsModel GetNewModelInstance()
        {
            return new BasicSvdBiasBinsModel();
        }
    }
}