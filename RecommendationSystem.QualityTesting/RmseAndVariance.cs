using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting
{
    public class RmseAndVariance
    {
        public float Rmse { get; set; }
        public float Variance { get; set; }

        public RmseAndVariance(float rmse, float variance)
        {
            Rmse = rmse;
            Variance = variance;
        }

        public RmseAndVariance(List<float> rmseList)
        {
            Rmse = rmseList.Average();
            Variance = rmseList.Select(rmse => Math.Abs(Rmse - rmse)).Average();
        }

        public override string ToString()
        {
            return string.Format("RMSE: {0}, Variance: {1}", Rmse, Variance);
        }
    }
}