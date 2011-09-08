using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.Models;

namespace RecommendationSystem.Svd.Foundation.Bias.Models
{
    public class BiasSvdModelPartLoader : IModelPartLoader
    {
        public Type Type
        {
            get { return typeof(IBiasSvdModel); }
        }

        public void LoadModelProperties(IModel model, TextReader reader)
        {
            var biasSvdModel = model as IBiasSvdModel;
            if (biasSvdModel == null)
                return;

            //get global average
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid IBiasSvdModel.");
            biasSvdModel.GlobalAverage = float.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1], CultureInfo.InvariantCulture);

            biasSvdModel.UserBias = new float[biasSvdModel.UserFeatures.GetUpperBound(1) + 1];
            biasSvdModel.ArtistBias = new float[biasSvdModel.ArtistFeatures.GetUpperBound(1) + 1];
        }

        public void LoadModelData(IModel model, TextReader reader)
        {
            var biasSvdModel = model as IBiasSvdModel;
            if (biasSvdModel == null)
                return;

            FillBiases(biasSvdModel.UserBias, reader);
            FillBiases(biasSvdModel.ArtistBias, reader);
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
                biases[i] = float.Parse(factors[i], CultureInfo.InvariantCulture);
        }
    }
}