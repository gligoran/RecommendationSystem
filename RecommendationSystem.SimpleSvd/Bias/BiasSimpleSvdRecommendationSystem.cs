using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.SimpleSvd.Bias.Recommendations;
using RecommendationSystem.SimpleSvd.Bias.Training;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd.Bias
{
    public class BiasSimpleSvdRecommendationSystem : SimpleSvdRecommendationSystemBase<IBiasSvdModel>
    {
        #region Constructor
        public BiasSimpleSvdRecommendationSystem()
        {
            Trainer = new BiasSimpleSvdTrainer();
            Recommender = new BiasSimpleSvdRecommender();
        }

        public BiasSimpleSvdRecommendationSystem(ISvdTrainer<IBiasSvdModel> trainer)
            : this(trainer, new BiasSimpleSvdRecommender())
        {}

        public BiasSimpleSvdRecommendationSystem(ISimpleSvdRecommender<IBiasSvdModel> recommender)
            : this(new BiasSimpleSvdTrainer(), recommender)
        {}

        public BiasSimpleSvdRecommendationSystem(ISvdTrainer<IBiasSvdModel> trainer, ISimpleSvdRecommender<IBiasSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
        #endregion

        #region SaveModel
        protected override void SaveProperties(IBiasSvdModel model, TextWriter writer)
        {
            base.SaveProperties(model, writer);

            writer.WriteLine("GlobalAverage={0}", model.GlobalAverage);
        }

        protected override void SaveData(IBiasSvdModel model, TextWriter writer)
        {
            base.SaveData(model, writer);

            SaveBiases(writer, model.UserBias);
            SaveBiases(writer, model.ArtistBias);
        }

        private static void SaveBiases(TextWriter writer, float[] biases)
        {
            for (var i = 0; i < biases.Length; i++)
            {
                if (i != 0)
                    writer.Write("\t");

                writer.Write(biases[i]);
            }
            writer.WriteLine();
        }
        #endregion

        #region LoadModel
        protected override IBiasSvdModel GetNewModel()
        {
            return new BiasSvdModel();
        }

        protected override void LoadProperties(IBiasSvdModel model, TextReader reader)
        {
            base.LoadProperties(model, reader);

            //get global average
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid IBiasSvdModel.");
            model.GlobalAverage = float.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1], CultureInfo.CurrentCulture);

            model.UserBias = new float[model.UserFeatures.GetUpperBound(1) + 1];
            model.ArtistBias = new float[model.ArtistFeatures.GetUpperBound(1) + 1];
        }

        protected override void LoadData(IBiasSvdModel model, TextReader reader)
        {
            base.LoadData(model, reader);

            FillBiases(model.UserBias, reader);
            FillBiases(model.ArtistBias, reader);
        }

        private void FillBiases(float[] biases, TextReader reader)
        {
            var sep = new[] {'\t'};
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File is not a valid IBiasSvdModel.");

            var factors = line.Split(sep, StringSplitOptions.None);
            if (factors.Length != biases.Length)
                throw new ArgumentException("File is not a valid IBiasSvdModel.");

            for (var i = 0; i < factors.Length; i++)
                biases[i] = float.Parse(factors[i], CultureInfo.CurrentCulture);
        }
        #endregion
    }
}