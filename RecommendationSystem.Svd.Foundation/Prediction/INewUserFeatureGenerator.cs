using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public interface INewUserFeatureGenerator<in TSvdModel>
        where TSvdModel : ISvdModel
    {
        float[] GetNewUserFeatures(TSvdModel model, IUser user);
    }
}