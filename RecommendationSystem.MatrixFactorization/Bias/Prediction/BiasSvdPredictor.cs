using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;

namespace RecommendationSystem.MatrixFactorization.Bias.Prediction
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

        public float PredictRating(IBiasSvdModel model, IUser user)
        {
            throw new NotImplementedException();

            /*var rating = 0.0f;
            for (var i = 0; i < model.UserTrainingParameters.FeatureCount; i++)
                rating += model.UserFeatures[i, userIndex] * model.ArtistFeatures[i, artistIndex];

            return rating + model.GlobalAverage + model.UserBias[userIndex] + model.ArtistBias[artistIndex];*/
        }
    }
}