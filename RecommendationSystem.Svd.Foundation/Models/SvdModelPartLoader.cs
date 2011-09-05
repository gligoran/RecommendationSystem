using System;
using System.Globalization;
using System.IO;
using RecommendationSystem.Models;

namespace RecommendationSystem.Svd.Foundation.Models
{
    public class SvdModelPartLoader : IModelPartLoader
    {
        public Type Type
        {
            get { return typeof(ISvdModel); }
        }

        public void LoadModelProperties(IModel model, TextReader reader)
        {
            var svdModel = model as ISvdModel;
            if (svdModel == null)
                return;

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
            svdModel.UserFeatures = new float[featureCount,userCount];

            //get artist count
            line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid ISvdModel.");
            var artistCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);
            svdModel.ArtistFeatures = new float[featureCount,artistCount];
        }

        public void LoadModelData(IModel model, TextReader reader)
        {
            var svdModel = model as ISvdModel;
            if (svdModel == null)
                return;

            FillFeatures(svdModel.UserFeatures, reader);
            FillFeatures(svdModel.ArtistFeatures, reader);
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
                    features[i, j] = float.Parse(factors[j], CultureInfo.InvariantCulture);
            }
        }
    }
}