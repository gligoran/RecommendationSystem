using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Training
{
    public abstract class SvdTrainerBase<TSvdModel> : ISvdTrainer<TSvdModel>
        where TSvdModel : ISvdModel
    {
        #region Properties
        protected float[] ResidualRatingValues;

        private float rmsePrev = float.MaxValue;
        private float rmse = float.MaxValue;
        #endregion

        #region TrainModel
        public TSvdModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            return TrainModel(users, artists, ratings, new TrainingParameters());
        }

        public TSvdModel TrainModel(IEnumerable<IUser> users, IEnumerable<IArtist> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            return TrainModel(users.GetLookupTable(), artists.GetLookupTable(), ratings, trainingParameters);
        }

        public TSvdModel TrainModel(List<string> users, List<string> artists, List<IRating> ratings, TrainingParameters trainingParameters)
        {
            var model = InitializeNewModel(users, artists, ratings);
            CalculateFeatures(model, users, artists, ratings, trainingParameters);
            return model;
        }

        protected abstract TSvdModel InitializeNewModel(List<string> users, List<string> artists, List<IRating> ratings);
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
#if DEBUG
                Console.WriteLine("Training feature {0}", f);
#endif

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

            while ((rmseImprovment > trainingParameters.RmseImprovementTreshold || count > trainingParameters.MinEpochTreshold) && count < trainingParameters.MaxEpochTreshold)
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

        #region TrainFeatures
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

            model.UserFeatures[f, ratings[r].UserIndex] += trainingParameters.LRate *
                                                           (e * model.ArtistFeatures[f, ratings[r].ArtistIndex] - trainingParameters.K * model.UserFeatures[f, ratings[r].UserIndex]);
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
            if (model.UserFeatures == null || model.ArtistFeatures == null)
                return;

            var writer = GetWriter(filename);

            SaveProperties(model, writer);
            SaveData(model, writer);

            EndSaving(writer);
        }

        private TextWriter GetWriter(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            TextWriter writer = new StreamWriter(filename);
            return writer;
        }

        protected virtual void SaveProperties(TSvdModel model, TextWriter writer)
        {
            writer.WriteLine("FeatureCount={0}", model.UserFeatures.GetUpperBound(0) + 1);
            writer.WriteLine("UserCount={0}", model.UserFeatures.GetUpperBound(1) + 1);
            writer.WriteLine("ArtistCount={0}", model.ArtistFeatures.GetUpperBound(1) + 1);
        }

        protected virtual void SaveData(TSvdModel model, TextWriter writer)
        {
            SaveFeatures(writer, model.UserFeatures);
            SaveFeatures(writer, model.ArtistFeatures);
        }

        private void SaveFeatures(TextWriter writer, float[,] features)
        {
            for (var i = 0; i <= features.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= features.GetUpperBound(1); j++)
                {
                    if (j != 0)
                        writer.Write("\t");

                    writer.Write(features[i, j]);
                }
                writer.WriteLine();
            }
        }

        private void EndSaving(TextWriter writer)
        {
            writer.Flush();
            writer.Close();
        }
        #endregion

        #region LoadModel
        public TSvdModel LoadModel(string filename)
        {
            TextReader reader = new StreamReader(filename);

            var model = GetNewModel();
            LoadProperties(model, reader);
            LoadData(model, reader);

            EndLoadingModel(reader);

            return model;
        }

        protected abstract TSvdModel GetNewModel();

        protected virtual void LoadProperties(TSvdModel model, TextReader reader)
        {
            //get feature count
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File is not a valide SvdModel.");
            var featureCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            //get user count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valide SvdModel.");
            var userCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            //get artist count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valide SvdModel.");
            var artistCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            model.UserFeatures = new float[featureCount,userCount];
            model.ArtistFeatures = new float[featureCount,artistCount];
        }

        protected virtual void LoadData(TSvdModel model, TextReader reader)
        {
            FillFeatures(model.UserFeatures, reader);
            FillFeatures(model.ArtistFeatures, reader);
        }

        private void FillFeatures(float[,] features, TextReader reader)
        {
            var sep = new[] {"\t"};
            for (var i = 0; i <= features.GetUpperBound(0); i++)
            {
                var line = reader.ReadLine();
                if (line == null)
                    throw new ArgumentException("File is not a valid SvdModel.");

                var factors = line.Split(sep, StringSplitOptions.None);
                if (factors.Length != features.GetUpperBound(1) + 1)
                    throw new ArgumentException("File is not a valid SvdModel.");

                for (var j = 0; j < factors.Length; j++)
                    features[i, j] = float.Parse(factors[j], CultureInfo.CurrentCulture);
            }
        }

        private void EndLoadingModel(TextReader reader)
        {
            reader.Close();
        }
        #endregion
    }
}