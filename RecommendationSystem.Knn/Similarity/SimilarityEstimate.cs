using System;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public class SimilarityEstimate : IComparable<SimilarityEstimate>
    {
        public User SimilarUser { get; set; }
        public float Estimate { get; set; }

        public SimilarityEstimate(User user, float estimate)
        {
            SimilarUser = user;
            Estimate = estimate;
        }

        public int CompareTo(SimilarityEstimate other)
        {
            return Estimate > other.Estimate ? -1 : (Estimate < other.Estimate ? 1 : 0);
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Estimate, SimilarUser);
        }
    }
}
