using System;
using System.IO;

namespace RecommendationSystem.Models
{
    public interface IModelPartLoader
    {
        Type Type { get; }

        void LoadModelProperties(IModel model, TextReader reader);
        void LoadModelData(IModel model, TextReader reader);
    }
}