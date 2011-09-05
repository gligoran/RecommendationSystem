using System;
using System.IO;

namespace RecommendationSystem.Models
{
    public interface IModelPartSaver
    {
        Type Type { get; }

        void SaveModelProperties(IModel model, TextWriter writer);
        void SaveModelData(IModel model, TextWriter writer);
    }
}