using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.Svd.Foundation.Basic.Prediction
{
    public class BasicNewUserFeatureGenerator : INewUserFeatureGenerator<IBasicSvdModel>
    {
        public float[] GetNewUserFeatures(IBasicSvdModel model, IUser user)
        {
            var ratingSum = user.Ratings.Sum(r => r.Value);

            var newUserFeatures = new float[model.FeatureCount];
            for (var f = 0; f < model.FeatureCount; f++)
            {
                newUserFeatures[f] = 0.0f;
                foreach (var rating in user.Ratings)
                    newUserFeatures[f] += rating.Value * model.ArtistFeatures[f, rating.ArtistIndex];

                newUserFeatures[f] /= ratingSum;
            }

            return newUserFeatures;
        }
    }
}