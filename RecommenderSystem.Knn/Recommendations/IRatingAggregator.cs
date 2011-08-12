using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecommenderSystem.Knn.Similarity;

namespace RecommenderSystem.Knn.Recommendations
{
    public interface IRatingAggregator
    {
        double Aggregate(User user, string artist);
    }
}
