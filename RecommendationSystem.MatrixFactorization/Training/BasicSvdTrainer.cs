using System.Collections.Generic;
using RecommendationSystem.Data;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public class BasicSvdTrainer : SvdTrainerBase<ISvdModel>
    {
        public BasicSvdTrainer(List<Rating> ratings, List<string> users, List<string> artists)
            : base(ratings, users, artists)
        { }

        public override ISvdModel TrainModel(TrainingParameters trainingParameters)
        {
            CalculateFeatures(trainingParameters);
            return new SvdModel(UserFeatures, ArtistFeatures, trainingParameters);
        }

        protected override float PredictRatingUsingResiduals(int rating, int feature)
        {
            return ResidualRatingValues[rating] + UserFeatures[feature, Ratings[rating].UserIndex] * ArtistFeatures[feature, Ratings[rating].ArtistIndex];
        }
    }
}
