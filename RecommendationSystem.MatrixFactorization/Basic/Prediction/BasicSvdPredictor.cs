using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Basic.Prediction
{
    public class BasicSvdPredictor : IBasicSvdPredictor
    {
        public List<string> Users { get; set; }
        public List<string> Artists { get; set; }

        public BasicSvdPredictor(List<string> users, List<string> artists)
        {
            Users = users;
            Artists = artists;
        }

        public float PredictRating(ISvdModel model, IUser user)
        {
            throw new NotImplementedException();

            /*var rating = 0.0f;
            for (var i = 0; i < model.UserTrainingParameters.FeatureCount; i++)
                rating += model.UserFeatures[i, userIndex] * model.ArtistFeatures[i, artistIndex];

            return rating;*/
        }
    }
}