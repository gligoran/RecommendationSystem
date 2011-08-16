using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RecommendationSystem.MatrixFactorization
{
    [Serializable]
    public class Rating : ISerializable
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

        public Rating(SerializationInfo info, StreamingContext context)
        {
            UserIndex = (int)info.GetValue("u", typeof(int));
            ArtistIndex = (int)info.GetValue("a", typeof(int));
            Value = (float)info.GetValue("u", typeof(float));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("u", UserIndex);
            info.AddValue("a", ArtistIndex);
            info.AddValue("v", Value);
        }
    }
}
