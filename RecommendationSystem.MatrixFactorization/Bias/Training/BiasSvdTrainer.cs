using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Bias.Training
{
    public class BiasSvdTrainer : SvdTrainerBase<IBiasSvdModel>
    {
        protected override IBiasSvdModel InitializeNewModel(List<string> users, List<string> artists, List<IRating> ratings)
        {
            var model = new BiasSvdModel();
            ComputeGlobalAverageAndBiases(model, users, artists, ratings);
            return model;
        }

        #region PredictRatingUsingResiduals
        protected override float PredictRatingUsingResiduals(IBiasSvdModel model, int rating, int feature, List<IRating> ratings)
        {
            return ResidualRatingValues[rating] +
                   model.UserFeatures[feature, ratings[rating].UserIndex] * model.ArtistFeatures[feature, ratings[rating].ArtistIndex] +
                   model.GlobalAverage +
                   model.UserBias[ratings[rating].UserIndex] +
                   model.ArtistBias[ratings[rating].ArtistIndex];
        }
        #endregion

        #region ComputeGlobalAverageAndBiases
        private void ComputeGlobalAverageAndBiases(IBiasSvdModel model, List<string> users, List<string> artists, List<IRating> ratings)
        {
            model.UserBias = new float[users.Count];
            model.ArtistBias = new float[artists.Count];

            model.GlobalAverage = ratings.Average(rating => rating.Value);

            var userCount = new int[users.Count];
            var artistCount = new int[artists.Count];
            foreach (var rating in ratings)
            {
                var d = rating.Value - model.GlobalAverage;

                model.UserBias[rating.UserIndex] += d;
                model.ArtistBias[rating.ArtistIndex] += d;

                userCount[rating.UserIndex] += 1;
                artistCount[rating.ArtistIndex] += 1;
            }

            for (var i = 0; i < model.UserBias.Length; i++)
                model.UserBias[i] /= userCount[i];

            for (var i = 0; i < model.ArtistBias.Length; i++)
                model.ArtistBias[i] /= artistCount[i];
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
                throw new ArgumentException("File {0} is not a valide SvdModel.");
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
                throw new ArgumentException("File is not a valid SvdModel.");

            var factors = line.Split(sep, StringSplitOptions.None);
            if (factors.Length != biases.Length)
                throw new ArgumentException("File is not a valid SvdModel.");

            for (var i = 0; i < factors.Length; i++)
                biases[i] = float.Parse(factors[i], CultureInfo.CurrentCulture);
        }
        #endregion
    }
}