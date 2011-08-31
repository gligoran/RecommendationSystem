using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting
{
    public class RmseBiasAndVariance
    {
        internal List<float> RmseList { get; set; }
        internal List<float> BiasList { get; set; }
        public float AverageRmse { get; set; }
        public float AverageBias { get; set; }
        public float RmseVariance { get; set; }
        public float EstimateVariance { get; set; }

        public RmseBiasAndVariance(List<float> rmseList, List<float> biasList)
        {
            RmseList = rmseList;
            AverageRmse = rmseList.Average();
            BiasList = biasList;
            AverageBias = biasList.Average();
            RmseVariance = rmseList.Sum(rmse => (float)Math.Pow(rmse - AverageRmse, 2)) / (rmseList.Count - 1);
            EstimateVariance = RmseVariance / rmseList.Count;
        }

        public RmseBiasAndVariance()
        {
            RmseList = new List<float>();
        }

        public override string ToString()
        {
            return string.Format("N: {0},\tAvgRMSE: {1},\tAvgBias: {2}\tRmseVar: {3},\tEstVar: {4}", RmseList.Count, AverageRmse, AverageBias, RmseVariance, EstimateVariance);
        }
    }
}