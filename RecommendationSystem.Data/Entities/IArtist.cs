using System;
using System.Collections.Generic;

namespace RecommendationSystem.Data.Entities
{
    public interface IArtist : IComparable<IArtist>, IEquatable<IArtist>
    {
        string Name { get; set; }
        List<IRating> Ratings { get; set; }
    }
}