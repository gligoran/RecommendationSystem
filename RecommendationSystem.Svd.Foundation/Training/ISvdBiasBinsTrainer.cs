using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public interface ISvdBiasBinsTrainer<TSvdBiasBinsModel> : ISvdTrainer<TSvdBiasBinsModel>, IBiasBinsTrainer<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {}
}