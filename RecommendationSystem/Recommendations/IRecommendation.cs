using System;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Recommendations
{
    public interface IRecommendation : IComparable<IRecommendation>
    {
        IArtist Artist { get; set; }
        float Value { get; set; }
    }
}