using RecommendationSystem.SimpleSvd.Prediction;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleSvd.Training
{
    public class SimpleSvdBiasBinsTrainer : SvdBiasBinsTrainerBase<ISvdBiasBinsModel>
    {
        public SimpleSvdBiasBinsTrainer()
            : this(new SvdBiasBinsCalculator<ISvdBiasBinsModel>(new SimpleSvdPredictor()))
        {}

        public SimpleSvdBiasBinsTrainer(IBiasBinsCalculator<ISvdBiasBinsModel> biasBinsCalculator)
            : base(biasBinsCalculator)
        {}

        protected override ISvdBiasBinsModel GetNewModelInstance()
        {
            return new SvdBiasBinsModel();
        }
    }
}