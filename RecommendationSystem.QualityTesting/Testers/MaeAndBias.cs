using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.QualityTesting.Testers
{
    internal class MaeAndBias
    {
        internal List<float> MaeList { get; set; }
        internal List<float> BiasList { get; set; }
        public float AverageMae { get; set; }
        public float AverageBias { get; set; }

        public MaeAndBias(List<float> maeList, List<float> biasList)
        {
            this.MaeList = maeList;
            AverageMae = maeList.Average();

            BiasList = biasList;
            AverageBias = biasList.Average();
        }

        public MaeAndBias()
        {
            MaeList = new List<float>();
            BiasList = new List<float>();
        }

        public override string ToString()
        {
            return string.Format("N: {0},\tAvgMAE: {1},\tAvgBias: {2}", MaeList.Count, AverageMae, AverageBias);
        }
    }
}