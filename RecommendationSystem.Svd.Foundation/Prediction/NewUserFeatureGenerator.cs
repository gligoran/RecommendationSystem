using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public class NewUserFeatureGenerator : INewUserFeatureGenerator<ISvdModel>
    {
        public float[] GetNewUserFeatures(ISvdModel model, IUser user)
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