using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd
{
    public abstract class SvdRecommendationSystemBase<TSvdModel> : ISvdRecommendationSystem<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public ISvdTrainer<TSvdModel> Trainer { get; set; }
        public ISvdRecommender<TSvdModel> Recommender { get; set; }

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
            writer.WriteLine("BiasBinCount={0}", model.BiasBins.Length);
        }

        protected virtual void SaveData(TSvdModel model, TextWriter writer)
        {
            SaveFeatures(writer, model.UserFeatures);
            SaveFeatures(writer, model.ArtistFeatures);
            SaveBiasBins(writer, model.BiasBins);
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

        private void SaveBiasBins(TextWriter writer, float[] biasBins)
        {
            for (var i = 0; i < biasBins.Length; i++)
            {
                if (i != 0)
                    writer.Write("\t");

                writer.Write(biasBins[i]);
            }
            writer.WriteLine();
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
                throw new ArgumentException("File is not a valid ISvdModel.");
            var featureCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            //get user count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid ISvdModel.");
            var userCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            //get artist count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid ISvdModel.");
            var artistCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            //get bias bins count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid ISvdModel.");
            var biasBinCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);

            model.UserFeatures = new float[featureCount,userCount];
            model.ArtistFeatures = new float[featureCount,artistCount];
            model.BiasBins = new float[biasBinCount];
        }

        protected virtual void LoadData(TSvdModel model, TextReader reader)
        {
            FillFeatures(model.UserFeatures, reader);
            FillFeatures(model.ArtistFeatures, reader);
            FillBiasBins(model.BiasBins, reader);
        }

        private void FillFeatures(float[,] features, TextReader reader)
        {
            var sep = new[] {"\t"};
            for (var i = 0; i <= features.GetUpperBound(0); i++)
            {
                var line = reader.ReadLine();
                if (line == null)
                    throw new ArgumentException("File is not a valid ISvdModel.");

                var factors = line.Split(sep, StringSplitOptions.None);
                if (factors.Length != features.GetUpperBound(1) + 1)
                    throw new ArgumentException("File is not a valid ISvdModel.");

                for (var j = 0; j < factors.Length; j++)
                    features[i, j] = float.Parse(factors[j], CultureInfo.CurrentCulture);
            }
        }

        private void FillBiasBins(float[] biasBins, TextReader reader)
        {
            var sep = new[] {'\t'};
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File is not a valid ISvdModel.");

            var factors = line.Split(sep, StringSplitOptions.None);
            if (factors.Length != biasBins.Length)
                throw new ArgumentException("File is not a valid ISvdModel.");

            for (var i = 0; i < factors.Length; i++)
                biasBins[i] = float.Parse(factors[i], CultureInfo.CurrentCulture);
        }

        private void EndLoadingModel(TextReader reader)
        {
            reader.Close();
        }
        #endregion
    }
}