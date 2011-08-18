using System.Collections.Generic;

namespace RecommendationSystem.MatrixFactorization.Learner
{
    public class BasicSvdLearner : SvdLearner
    {
        public BasicSvdLearner(List<Rating> ratings, List<string> users, List<string> artists)
            : base(ratings, users, artists)
        { }

        protected override float PredictRatingWithResiduals(int rating, int feature)
        {
            return ResidualRatingValues[rating] + UserFeatures[feature, Ratings[rating].UserIndex] * ArtistFeatures[feature, Ratings[rating].ArtistIndex];
        }

        public override float PredictRating(int u, int a)
        {
            var rating = 0.0f;
            for (var i = 0; i < LearningParameters.FeatureCount; i++)
                rating += UserFeatures[i, u] * ArtistFeatures[i, a];

            return rating;
        }
    }
}
