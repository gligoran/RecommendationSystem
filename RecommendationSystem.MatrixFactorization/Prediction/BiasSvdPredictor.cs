using System.Collections.Generic;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public class BiasSvdPredictor : IBiasSvdPredictor
    {
        public List<string> Users { get; set; }
        public List<string> Artists { get; set; }

        public BiasSvdPredictor(List<string> users, List<string> artists)
        {
            Users = users;
            Artists = artists;
        }

        public float PredictRating(IBiasSvdModel model, string user, string artist)
        {
            return PredictRating(model, Users.BinarySearch(user), Artists.BinarySearch(artist));
        }

        public float PredictRating(IBiasSvdModel model, string user, int artistIndex)
        {
            return PredictRating(model, Users.BinarySearch(user), artistIndex);
        }

        public float PredictRating(IBiasSvdModel model, int userIndex, string artist)
        {
            return PredictRating(model, userIndex, Artists.BinarySearch(artist));
        }

        public float PredictRating(IBiasSvdModel model, int userIndex, int artistIndex)
        {
            var rating = 0.0f;
            for (var i = 0; i < model.TrainingParameters.FeatureCount; i++)
                rating += model.UserFeatures[i, userIndex] * model.ArtistFeatures[i, artistIndex];

            return rating + model.GlobalAverage + model.UserBias[userIndex] + model.ArtistBias[artistIndex];
        }
    }
}