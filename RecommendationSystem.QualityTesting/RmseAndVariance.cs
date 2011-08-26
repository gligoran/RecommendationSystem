using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting
{
    public class RmseAndVariance
    {
        internal List<float> RmseList { get; set; }
        public float AverageRmse { get; set; }
        public float RmseVariance { get; set; }
        public float EstimateVariance { get; set; }

        public RmseAndVariance(List<float> rmseList)
        {
            RmseList = rmseList;
            AverageRmse = rmseList.Average();
            RmseVariance = rmseList.Sum(rmse => (float)Math.Pow(rmse - AverageRmse, 2)) / (rmseList.Count - 1);
            EstimateVariance = RmseVariance / rmseList.Count;
        }

        public override string ToString()
        {
            return string.Format("N: {0},\tAvgRMSE: {1},\tRmseVar: {2},\tEstVar: {3}", RmseList.Count, AverageRmse, RmseVariance, EstimateVariance);
        }
    }
}