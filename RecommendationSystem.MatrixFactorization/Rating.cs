using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RecommendationSystem.MatrixFactorization
{
    public class Rating
    {
        public int UserIndex { get; set; }
        public int ArtistIndex { get; set; }
        public float Value { get; set; }

        public Rating() { }

        public Rating(int userIndex, int artistIndex, float value)
        {
            this.UserIndex = userIndex;
            this.ArtistIndex = artistIndex;
            this.Value = value;
        }
    }
}
