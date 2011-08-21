using System;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class SimilarUser : IComparable<SimilarUser>
    {
        public IKnnUser User { get; set; }
        public float Similarity { get; set; }

        public SimilarUser(IKnnUser user, float similarity)
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