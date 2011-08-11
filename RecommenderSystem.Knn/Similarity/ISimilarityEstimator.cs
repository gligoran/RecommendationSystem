using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Knn.Similarity
{
    public interface ISimilarityEstimator
    {
        double Similarity(User first, User second);
    }
}
