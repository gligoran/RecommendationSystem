using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Training
{
    public abstract class SvdTrainerBase<TSvdModel> : ISvdTrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        #region Properties
        protected float[] ResidualRatingValues { get; set; }
        protected ModelSaver ModelSaver { get; set; }
        #endregion

        #region Fields
        private float rmsePrev = float.MaxValue;
        private float rmse = float.MaxValue;
        #endregion

        protected SvdTrainerBase()
        {
            ModelSaver = new ModelSaver();
            ModelSaver.ModelPartSavers.Add(new SvdModelPartSaver());
        }

        #region TrainModel
        public TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            return TrainModel(users, artists, ratings, new TrainingParameters());
        }

        public TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            return TrainModel(users.GetLookupTable(), artists.GetLookupTable(), ratings, trainingParameters);
        }

        protected TSvdModel TrainModel(List<string> users, List<string> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var model = GetNewModelInstance();
            InitializeNewModel(model, users, artists, ratings);

            CalculateFeatures(model, users, artists, ratings, trainingParameters);
            return model;
        }

        protected abstract TSvdModel GetNewModelInstance();

        protected virtual void InitializeNewModel(TSvdModel model, List<string> users, List<string> artists, List<IRating> ratings)
        {}
        #endregion

        #region CalculateFeatures
        protected void CalculateFeatures(TSvdModel model, List<string> users, List<string> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            //init
            model.UserFeatures = new float[trainingParameters.FeatureCount,users.Count];
            model.ArtistFeatures = new float[trainingParameters.FeatureCount,artists.Count];

            ResidualRatingValues = new float[ratings.Count];
            model.UserFeatures.Populate(0.1f);
            model.ArtistFeatures.Populate(0.1f);

            rmsePrev = float.MaxValue;
            rmse = float.MaxValue;

            //MAIN LOOP - loops through features
            for (var f = 0; f < trainingParameters.FeatureCount; f++)
            {
                Console.WriteLine("Training feature {0} ({1})", f, DateTime.Now);

                ConvergeFeature(model, f, ratings, trainingParameters);
                CacheResidualRatings(model, f, ratings);
            }
        }
        #endregion

        #region ConvergeFeature
        private void ConvergeFeature(TSvdModel model, int f, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var count = 0;
            var rmseImprovment = float.MaxValue;

            while ((rmseImprovment > trainingParameters.RmseImprovementTreshold || count < trainingParameters.MinEpochTreshold) && count < trainingParameters.MaxEpochTreshold)
            {
                rmsePrev = rmse;
                rmse = TrainFeature(model, f, ratings, trainingParameters);
                rmseImprovment = Math.Abs(rmse - rmsePrev) / (rmse + rmsePrev);

                count++;
                Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseImpr = {3}", f, count, rmse, rmseImprovment);
            }

            rmsePrev = rmse;
        }
        #endregion

        #region TrainFeature
        protected float TrainFeature(TSvdModel model, int f, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var e = ratings.Select((r, i) => TrainSample(model, i, f, ratings, trainingParameters)).Sum() / ratings.Count;
            return (float)Math.Sqrt(e);
        }
        #endregion

        #region TrainSample
        protected float TrainSample(TSvdModel model, int r, int f, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var e = ratings[r].Value - PredictRatingUsingResiduals(model, r, f, ratings);
            var uv = model.UserFeatures[f, ratings[r].UserIndex];

            model.UserFeatures[f, ratings[r].UserIndex] += trainingParameters.LRate * (e * model.ArtistFeatures[f, ratings[r].ArtistIndex] - trainingParameters.K * model.UserFeatures[f, ratings[r].UserIndex]);
            model.ArtistFeatures[f, ratings[r].ArtistIndex] += trainingParameters.LRate * (e * uv - trainingParameters.K * model.ArtistFeatures[f, ratings[r].ArtistIndex]);

            return e * e;
        }
        #endregion

        #region PredictRatingUsingResiduals
        protected abstract float PredictRatingUsingResiduals(TSvdModel model, int rating, int feature, List<IRating> ratings);
        #endregion

        #region CacheResidualRatings
        private void CacheResidualRatings(TSvdModel model, int f, List<IRating> ratings)
        {
            for (var i = 0; i < ratings.Count; i++)
                ResidualRatingValues[i] += model.UserFeatures[f, ratings[i].UserIndex] * model.ArtistFeatures[f, ratings[i].ArtistIndex];
        }
        #endregion

        #region SaveModel
        public void SaveModel(string filename, TSvdModel model)
        {
            ModelSaver.SaveModel(filename, model);
        }
        #endregion
    }
}