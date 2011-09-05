using System;
using System.Globalization;
using System.IO;

namespace RecommendationSystem.Models
{
    public class BiasBinsModelPartSaver : IModelPartSaver
    {
        public Type Type
        {
            get { return typeof(IBiasBinsModel); }
        }

        public void SaveModelProperties(IModel model, TextWriter writer)
        {
            var biasBinsModel = model as IBiasBinsModel;
            if (biasBinsModel == null)
                return;

            writer.WriteLine("BiasBinCount={0}", biasBinsModel.BiasBins.Length);
        }

        public void SaveModelData(IModel model, TextWriter writer)
        {
            var biasBinsModel = model as IBiasBinsModel;
            if (biasBinsModel == null)
                return;

            SaveBiasBins(writer, biasBinsModel.BiasBins);
        }

        private void SaveBiasBins(TextWriter writer, float[] biasBins)
        {
            for (var i = 0; i < biasBins.Length; i++)
            {
                if (i != 0)
                    writer.Write("\t");

                writer.Write(biasBins[i].ToString(CultureInfo.InvariantCulture));
            }
            writer.WriteLine();
        }
    }
}