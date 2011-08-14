using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommendationSystem.Knn.Similarity
{
    public class SimilarityEstimate : IComparable<SimilarityEstimate>
    {
        public User SimilarUser { get; set; }
        public double Estimate { get; set; }

        public SimilarityEstimate(User user, double estimate)
        {
            this.SimilarUser = user;
            this.Estimate = estimate;
        }

        public int CompareTo(SimilarityEstimate other)
        {
            if (this.Estimate > other.Estimate)
                return -1;

            if (this.Estimate < other.Estimate)
                return 1;

            return 0;
        }
        
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Estimate, SimilarUser);
        }
    }
}
