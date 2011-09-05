using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.Models;

namespace RecommendationSystem.Svd.Foundation.Models
{
    public class SvdModelPartSaver : IModelPartSaver
    {
        public Type Type
        {
            get { return typeof(ISvdModel); }
        }

        public void SaveModelProperties(IModel model, TextWriter writer)
        {
            var svdModel = model as ISvdModel;
            if (svdModel == null)
                return;

            writer.WriteLine("FeatureCount={0}", svdModel.UserFeatures.GetUpperBound(0) + 1);
            writer.WriteLine("UserCount={0}", svdModel.UserFeatures.GetUpperBound(1) + 1);
            writer.WriteLine("ArtistCount={0}", svdModel.ArtistFeatures.GetUpperBound(1) + 1);
        }

        public void SaveModelData(IModel model, TextWriter writer)
        {
            var svdModel = model as ISvdModel;
            if (svdModel == null)
                return;

            SaveFeatures(writer, svdModel.UserFeatures);
            SaveFeatures(writer, svdModel.ArtistFeatures);
        }

        private void SaveFeatures(TextWriter writer, float[,] features)
        {
            for (var i = 0; i <= features.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= features.GetUpperBound(1); j++)
                {
                    if (j != 0)
                        writer.Write("\t");

                    writer.Write(features[i, j].ToString(CultureInfo.InvariantCulture));
                }
                writer.WriteLine();
            }
        }
    }
}