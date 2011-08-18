using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public abstract class SvdTrainerBase<TSvdModel> : ISvdTrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        protected float[,] UserFeatures;
        protected float[,] ArtistFeatures;
        protected float[] ResidualRatingValues;
        public List<string> Users { get; set; }
        public List<string> Artists { get; set; }
        public List<Rating> Ratings { get; set; }
        
        private float rmsePrev = float.MaxValue;

        protected SvdTrainerBase(List<Rating> ratings, List<string> users, List<string> artists)
        {
            Ratings = ratings;
            Users = users;
            Artists = artists;
        }

        public abstract TSvdModel TrainModel(TrainingParameters trainingParameters);

        protected void CalculateFeatures(TrainingParameters trainingParameters)
        {
            //init
            UserFeatures = new float[trainingParameters.FeatureCount, Users.Count];
            ArtistFeatures = new float[trainingParameters.FeatureCount, Artists.Count];
            ResidualRatingValues = new float[Ratings.Count];
            UserFeatures.Populate(0.1f);
            ArtistFeatures.Populate(0.1f);

            //MAIN LOOP - loops through features
            for (var f = 0; f < trainingParameters.FeatureCount; f++)
            {
#if DEBUG
                Console.WriteLine("Training feature {0}", f);
#endif

                ConvergeFeature(f, trainingParameters);
                CacheResidualRatings(f);
            }
        }

        private void ConvergeFeature(int f, TrainingParameters trainingParameters)
        {
            var count = 0;
            var rmseDiff = float.MaxValue;
            var rmse = float.MaxValue;

            while (rmseDiff > trainingParameters.RmseDiffLimit /*&& count < TrainingParameters.EpochLimit*/)
            {
                rmsePrev = rmse;
                rmse = TrainFeature(f, trainingParameters);
                rmseDiff = rmsePrev - rmse;

#if DEBUG
                count++;
                Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseDiff = {3}", f, count, rmse, rmseDiff);
#endif
            }

            rmsePrev = rmse;
        }

        protected float TrainFeature(int f, TrainingParameters trainingParameters)
        {
            var e = Ratings.Select((r, i) => TrainSample(i, f, trainingParameters)).Sum() / Ratings.Count;
            return (float)Math.Sqrt(e);
        }

        protected float TrainSample(int r, int f, TrainingParameters trainingParameters)
        {
            var e = Ratings[r].Value - PredictRatingUsingResiduals(r, f);
            var uv = UserFeatures[f, Ratings[r].UserIndex];

            UserFeatures[f, Ratings[r].UserIndex] += trainingParameters.LRate * (e * ArtistFeatures[f, Ratings[r].ArtistIndex] - trainingParameters.K * UserFeatures[f, Ratings[r].UserIndex]);
            ArtistFeatures[f, Ratings[r].ArtistIndex] += trainingParameters.LRate * (e * uv - trainingParameters.K * ArtistFeatures[f, Ratings[r].ArtistIndex]);

            return e * e;
        }

        protected abstract float PredictRatingUsingResiduals(int rating, int feature);

        private void CacheResidualRatings(int f)
        {
            for (var i = 0; i < Ratings.Count; i++)
                ResidualRatingValues[i] += UserFeatures[f, Ratings[i].UserIndex] * ArtistFeatures[f, Ratings[i].ArtistIndex];
        }
    }

}