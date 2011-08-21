using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public abstract class SvdTrainerBase<TSvdModel> : ISvdTrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        protected float[] ResidualRatingValues;
        public List<string> Users { get; set; }
        public List<string> Artists { get; set; }
        public List<IRating> Ratings { get; set; }

        private float rmsePrev = float.MaxValue;
        private float rmse = float.MaxValue;

        protected SvdTrainerBase(List<IRating> ratings, List<string> users, List<string> artists)
        {
            Ratings = ratings;
            Users = users;
            Artists = artists;
        }

        public TSvdModel TrainModel()
        {
            return TrainModel(new TrainingParameters());
        }

        public abstract TSvdModel TrainModel(TrainingParameters trainingParameters);

        protected void CalculateFeatures(TSvdModel model, TrainingParameters trainingParameters)
        {
            //init
            model.UserFeatures = new float[trainingParameters.FeatureCount,Users.Count];
            model.ArtistFeatures = new float[trainingParameters.FeatureCount,Artists.Count];

            ResidualRatingValues = new float[Ratings.Count];
            model.UserFeatures.Populate(0.1f);
            model.ArtistFeatures.Populate(0.1f);

            rmsePrev = float.MaxValue;
            rmse = float.MaxValue;

            //MAIN LOOP - loops through features
            for (var f = 0; f < trainingParameters.FeatureCount; f++)
            {
#if DEBUG
                Console.WriteLine("Training feature {0}", f);
#endif

                ConvergeFeature(model, f, trainingParameters);
                CacheResidualRatings(model, f);
            }
        }

        private void ConvergeFeature(TSvdModel model, int f, TrainingParameters trainingParameters)
        {
            var count = 0;
            var rmseDiff = float.MaxValue;

            while (count < trainingParameters.EpochLimit && rmseDiff > 0.0f)
            {
                rmsePrev = rmse;
                rmse = TrainFeature(model, f, trainingParameters);
                rmseDiff = rmsePrev - rmse;

#if DEBUG
                count++;
                Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseDiff = {3}", f, count, rmse, rmseDiff);
#endif
            }

            rmsePrev = rmse;
        }

        protected float TrainFeature(TSvdModel model, int f, TrainingParameters trainingParameters)
        {
            var e = Ratings.Select((r, i) => TrainSample(model, i, f, trainingParameters)).Sum() / Ratings.Count;
            return (float)Math.Sqrt(e);
        }

        protected float TrainSample(TSvdModel model, int r, int f, TrainingParameters trainingParameters)
        {
            var e = Ratings[r].Value - PredictRatingUsingResiduals(model, r, f);
            var uv = model.UserFeatures[f, Ratings[r].UserIndex];

            model.UserFeatures[f, Ratings[r].UserIndex] += trainingParameters.LRate *
                                                           (e * model.ArtistFeatures[f, Ratings[r].ArtistIndex] - trainingParameters.K * model.UserFeatures[f, Ratings[r].UserIndex]);
            model.ArtistFeatures[f, Ratings[r].ArtistIndex] += trainingParameters.LRate * (e * uv - trainingParameters.K * model.ArtistFeatures[f, Ratings[r].ArtistIndex]);

            return e * e;
        }

        protected abstract float PredictRatingUsingResiduals(TSvdModel model, int rating, int feature);

        private void CacheResidualRatings(TSvdModel model, int f)
        {
            for (var i = 0; i < Ratings.Count; i++)
                ResidualRatingValues[i] += model.UserFeatures[f, Ratings[i].UserIndex] * model.ArtistFeatures[f, Ratings[i].ArtistIndex];
        }
    }
}