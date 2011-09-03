using System;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.Knn.Foundation.Similarity
{
    public class SimilarUser<TKnnUser> : IComparable<SimilarUser<TKnnUser>>
        where TKnnUser : IKnnUser
    {
        public TKnnUser User { get; set; }
        public float Similarity { get; set; }

        public SimilarUser(TKnnUser user, float similarity)
        {
            User = user;
            Similarity = similarity;
        }

        public int CompareTo(SimilarUser<TKnnUser> other)
        {
            return -Similarity.CompareTo(other.Similarity);
        }
    }
}