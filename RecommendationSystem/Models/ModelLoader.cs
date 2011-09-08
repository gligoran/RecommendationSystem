using System.Collections.Generic;
using System.IO;

namespace RecommendationSystem.Models
{
    public class ModelLoader<TModel>
        where TModel : IModel
    {
        public List<IModelPartLoader> ModelPartLoaders { get; set; }

        public ModelLoader()
        {
            ModelPartLoaders = new List<IModelPartLoader>();
        }

        public void LoadModel(TModel model, string filename)
        {
            TextReader reader = new StreamReader(filename);

            LoadModelProperties(model, reader);
            LoadModelData(model, reader);

            reader.Close();
        }

        private void LoadModelProperties(TModel model, TextReader reader)
        {
            foreach (var modelPartLoader in ModelPartLoaders)
                modelPartLoader.LoadModelProperties(model, reader);
        }

        private void LoadModelData(TModel model, TextReader reader)
        {
            foreach (var modelPartLoader in ModelPartLoaders)
                modelPartLoader.LoadModelData(model, reader);
        }
    }
}