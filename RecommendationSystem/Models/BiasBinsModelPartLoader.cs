using System;
using System.Globalization;
using System.IO;

namespace RecommendationSystem.Models
{
    public class BiasBinsModelPartLoader : IModelPartLoader
    {
        public Type Type
        {
            get { return typeof(IBiasBinsModel); }
        }

        public void LoadModelProperties(IModel model, TextReader reader)
        {
            var biasBinsModel = model as IBiasBinsModel;
            if (biasBinsModel == null)
                return;

            //get bias bins count
            var line = reader.ReadLine();
            if (line == null)
                throw new ArgumentException("File {0} is not a valid ISvdModel.");
            var biasBinCount = int.Parse(line.Split(new[] {'='}, StringSplitOptions.None)[1]);
            biasBinsModel.BiasBins = new float[biasBinCount];
        }

        public void LoadModelData(IModel model, TextReader reader)
        {
            var biasBinsModel = model as IBiasBinsModel;
            if (biasBinsModel == null)
                return;

            FillBiasBins(biasBinsModel.BiasBins, reader);
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
                biasBins[i] = float.Parse(factors[i], CultureInfo.InvariantCulture);
        }
    }
}