using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class MaeBiasAndVariance
    {
        internal List<float> MaeList { get; set; }
        internal List<float> BiasList { get; set; }
        public float AverageMae { get; set; }
        public float AverageBias { get; set; }
        public float MaeVariance { get; set; }
        public float EstimateVariance { get; set; }

        public MaeBiasAndVariance(List<float> maeList, List<float> biasList)
        {
            MaeList = maeList;
            AverageMae = maeList.Average();
            BiasList = biasList;
            AverageBias = biasList.Average();
            MaeVariance = maeList.Sum(mae => (float)Math.Pow(mae - AverageMae, 2)) / (maeList.Count - 1);
            EstimateVariance = MaeVariance / maeList.Count;
        }

        public MaeBiasAndVariance()
        {
            MaeList = new List<float>();
        }

        public override string ToString()
        {
            return string.Format("N: {0},\tAvgMAE: {1},\tAvgBias: {2}\tMaeVar: {3},\tEstVar: {4}", MaeList.Count, AverageMae, AverageBias, MaeVariance, EstimateVariance);
        }
    }
}