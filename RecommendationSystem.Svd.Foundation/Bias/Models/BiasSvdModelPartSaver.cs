using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.Models;

namespace RecommendationSystem.Svd.Foundation.Bias.Models
{
    public class BiasSvdModelPartSaver : IModelPartSaver
    {
        public Type Type
        {
            get { return typeof(IBiasSvdModel); }
        }

        public void SaveModelProperties(IModel model, TextWriter writer)
        {
            var biasSvdModel = model as IBiasSvdModel;
            if (biasSvdModel == null)
                return;

            writer.WriteLine("GlobalAverage={0}", biasSvdModel.GlobalAverage.ToString(CultureInfo.InvariantCulture));
        }

        public void SaveModelData(IModel model, TextWriter writer)
        {
            var biasSvdModel = model as IBiasSvdModel;
            if (biasSvdModel == null)
                return;

            SaveBiases(writer, biasSvdModel.UserBias);
            SaveBiases(writer, biasSvdModel.ArtistBias);
        }

        private static void SaveBiases(TextWriter writer, float[] biases)
        {
            for (var i = 0; i < biases.Length; i++)
            {
                if (i != 0)
                    writer.Write("\t");

                writer.Write(biases[i].ToString(CultureInfo.InvariantCulture));
            }
            writer.WriteLine();
        }
    }
}