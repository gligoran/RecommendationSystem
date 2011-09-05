using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.Svd.Foundation.Bias.Prediction
{
    public class BiasNewUserFeatureGenerator : INewUserFeatureGenerator<IBiasSvdModel>
    {
        public float[] GetNewUserFeatures(IBiasSvdModel model, IUser user)
        {
            var userBias = user.Ratings.Average(rating => rating.Value - model.GlobalAverage);
            var ratingSum = user.Ratings.Sum(r => r.Value);
            var newUserFeatures = new float[model.FeatureCount];
            for (var f = 0; f < model.FeatureCount; f++)
            {
                newUserFeatures[f] = 0.0f;
                foreach (var rating in user.Ratings)
                    newUserFeatures[f] += (rating.Value - model.GlobalAverage - userBias - model.ArtistBias[rating.ArtistIndex]) * model.ArtistFeatures[f, rating.ArtistIndex];

                newUserFeatures[f] /= ratingSum;
            }

            return newUserFeatures;
        }
    }
}