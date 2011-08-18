using System.Collections.Generic;
using RecommendationSystem.Data;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public class BiasSvdTrainer : SvdTrainerBase<IBiasSvdModel>
    {
        private float globalAverage;
        private float[] userBias;
        private float[] artistBias;

        public BiasSvdTrainer(List<Rating> ratings, List<string> users, List<string> artists)
            : base(ratings, users, artists)
        {
            ComputeBiases();
        }

        public override IBiasSvdModel TrainModel(TrainingParameters trainingParameters)
        {
            CalculateFeatures(trainingParameters);
            return new BiasSvdModel(UserFeatures, ArtistFeatures, globalAverage, userBias, artistBias, trainingParameters);

        }

        protected override float PredictRatingUsingResiduals(int rating, int feature)
        {
            return ResidualRatingValues[rating] +
                   UserFeatures[feature, Ratings[rating].UserIndex] * ArtistFeatures[feature, Ratings[rating].ArtistIndex] +
                   globalAverage +
                   userBias[Ratings[rating].UserIndex] +
                   artistBias[Ratings[rating].ArtistIndex];
        }

        private void ComputeBiases()
        {
            globalAverage = 0.0f;
            userBias = new float[Users.Count];
            artistBias = new float[Artists.Count];

            foreach (var rating in Ratings)
                globalAverage += rating.Value;

            globalAverage /= Ratings.Count;

            var userCount = new int[Users.Count];
            var artistCount = new int[Artists.Count];
            foreach (var rating in Ratings)
            {
                var d = rating.Value - globalAverage;

                userBias[rating.UserIndex] += d;
                artistBias[rating.ArtistIndex] += d;

                userCount[rating.UserIndex] += 1;
                artistCount[rating.ArtistIndex] += 1;
            }

            for (var i = 0; i < userBias.Length; i++)
                userBias[i] /= userCount[i];

            for (var i = 0; i < artistBias.Length; i++)
                artistBias[i] /= artistCount[i];
        }
    }
}
