using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public class BiasSvdTrainer : SvdTrainerBase<IBiasSvdModel>
    {
        public override IBiasSvdModel TrainModel(List<string> users, List<string> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var model = new BiasSvdModel(trainingParameters);
            ComputeBiases(model);
            CalculateFeatures(model, trainingParameters);
            return model;
        }

        protected override float PredictRatingUsingResiduals(IBiasSvdModel model, int rating, int feature)
        {
            return ResidualRatingValues[rating] +
                   model.UserFeatures[feature, Ratings[rating].UserIndex] * model.ArtistFeatures[feature, Ratings[rating].ArtistIndex] +
                   model.GlobalAverage +
                   model.UserBias[Ratings[rating].UserIndex] +
                   model.ArtistBias[Ratings[rating].ArtistIndex];
        }

        private void ComputeBiases(IBiasSvdModel model)
        {
            model.GlobalAverage = 0.0f;
            model.UserBias = new float[Users.Count];
            model.ArtistBias = new float[Artists.Count];

            foreach (var rating in Ratings)
                model.GlobalAverage += rating.Value;

            model.GlobalAverage /= Ratings.Count;

            var userCount = new int[Users.Count];
            var artistCount = new int[Artists.Count];
            foreach (var rating in Ratings)
            {
                var d = rating.Value - model.GlobalAverage;

                model.UserBias[rating.UserIndex] += d;
                model.ArtistBias[rating.ArtistIndex] += d;

                userCount[rating.UserIndex] += 1;
                artistCount[rating.ArtistIndex] += 1;
            }

            for (var i = 0; i < model.UserBias.Length; i++)
                model.UserBias[i] /= userCount[i];

            for (var i = 0; i < model.ArtistBias.Length; i++)
                model.ArtistBias[i] /= artistCount[i];
        }
    }
}