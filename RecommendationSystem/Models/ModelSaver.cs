using System.Collections.Generic;
using System.IO;

namespace RecommendationSystem.Models
{
    public class ModelSaver
    {
        public List<IModelPartSaver> ModelPartSavers { get; set; }

        public ModelSaver()
        {
            ModelPartSavers = new List<IModelPartSaver>();
        }

        public void SaveModel(string filename, IModel model)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            TextWriter writer = new StreamWriter(filename);

            SaveModelProperties(model, writer);
            SaveModelData(model, writer);

            writer.Flush();
            writer.Close();
        }

        private void SaveModelProperties(IModel model, TextWriter writer)
        {
            foreach (var modelPartSaver in ModelPartSavers)
                modelPartSaver.SaveModelProperties(model, writer);
        }

        private void SaveModelData(IModel model, TextWriter writer)
        {
            foreach (var modelPartSaver in ModelPartSavers)
                modelPartSaver.SaveModelData(model, writer);
        }
    }
}