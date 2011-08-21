using System;
using System.Collections.Generic;

namespace RecommendationSystem.Entities
{
    public interface IArtist : IComparable<IArtist>, IEquatable<IArtist>
    {
        string Name { get; set; }
        List<IRating> Ratings { get; set; }
    }
}