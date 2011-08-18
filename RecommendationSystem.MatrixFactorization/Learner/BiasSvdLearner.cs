using System.Collections.Generic;

namespace RecommendationSystem.MatrixFactorization.Learner
{
    public class BiasSvdLearner : SvdLearner
    {
        private float globalAverage;
        private float[] userBias;
        private float[] artistBias;

        public BiasSvdLearner(List<Rating> ratings, List<string> users, List<string> artists)
            : base(ratings, users, artists)
        {
            ComputeBiases();
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

        protected override float PredictRatingWithResiduals(int rating, int feature)
        {
            return ResidualRatingValues[rating] +
                   UserFeatures[feature, Ratings[rating].UserIndex] * ArtistFeatures[feature, Ratings[rating].ArtistIndex] +
                   globalAverage + 
                   userBias[Ratings[rating].UserIndex] + 
                   artistBias[Ratings[rating].ArtistIndex];
        }

        public override float PredictRating(int u, int a)
        {
            var rating = 0.0f;
            for (var i = 0; i < LearningParameters.FeatureCount; i++)
                rating += UserFeatures[i, u] * ArtistFeatures[i, a];

            return rating + globalAverage + userBias[u] + artistBias[a];
        }
    }
}
