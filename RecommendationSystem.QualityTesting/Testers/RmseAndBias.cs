using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting.Testers
{
    internal class RmseAndBias
    {
        internal List<float> RmseList { get; set; }
        internal List<float> BiasList { get; set; }
        public float AverageRmse { get; set; }
        public float AverageBias { get; set; }

        public RmseAndBias(List<float> rmseList, List<float> biasList)
        {
            RmseList = rmseList;
            AverageRmse = rmseList.Average();

            BiasList = biasList;
            AverageBias = biasList.Average();
        }

        public RmseAndBias()
        {
            RmseList = new List<float>();
            BiasList = new List<float>();
        }

        public override string ToString()
        {
            return string.Format("N: {0},\tAvgRMSE: {1},\tAvgBias: {2}", RmseList.Count, AverageRmse, AverageBias);
        }
    }
}