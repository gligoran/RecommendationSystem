using System;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Similarity
{
    public class SimilarUser : IComparable<SimilarUser>
    {
        public ISimpleKnnUser User { get; set; }
        public float Similarity { get; set; }

        public SimilarUser(ISimpleKnnUser user, float similarity)
        {
            User = user;
            Similarity = similarity;
        }

        public int CompareTo(SimilarUser other)
        {
            return -Similarity.CompareTo(other.Similarity);
        }
    }
}