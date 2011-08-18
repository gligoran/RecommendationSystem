using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.MatrixFactorization.Learner
{
    public abstract class SvdLearner
    {
        protected static float[,] UserFeatures;
        protected static float[,] ArtistFeatures;
        protected static float[] ResidualRatingValues;
        public List<string> Users { get; set; }
        public List<string> Artists { get; set; }
        public List<Rating> Ratings { get; set; }

        protected SvdLearner(List<Rating> ratings, List<string> users, List<string> artists)
        {
            Ratings = ratings;
            Users = users;
            Artists = artists;
        }

        public void Learn()
        {
            //init
            UserFeatures = new float[LearningParameters.FeatureCount, Users.Count];
            ArtistFeatures = new float[LearningParameters.FeatureCount, Artists.Count];
            ResidualRatingValues = new float[Ratings.Count];
            UserFeatures.Populate(0.1f);
            ArtistFeatures.Populate(0.1f);

            //MAIN LOOP - loops through features
            for (var f = 0; f < LearningParameters.FeatureCount; f++)
            {
#if DEBUG
                Console.WriteLine("Training feature {0}", f);
#endif

                ConvergeFeature(f);
                CacheResidualRatings(f);
            }
        }

        private float rmsePrev = float.MaxValue;
        private void ConvergeFeature(int f)
        {
            var count = 0;
            var rmseDiff = float.MaxValue;
            var rmse = float.MaxValue;

            while (rmseDiff > LearningParameters.RmseDiffLimit /*&& count < LearningParameters.EpochLimit*/)
            {
                rmsePrev = rmse;
                rmse = TrainFeature(f);
                rmseDiff = rmsePrev - rmse;

#if DEBUG
                count++;
                Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseDiff = {3}", f, count, rmse, rmseDiff);
#endif
            }

            rmsePrev = rmse;
        }

        private void CacheResidualRatings(int f)
        {
            for (var i = 0; i < Ratings.Count; i++)
                ResidualRatingValues[i] += UserFeatures[f, Ratings[i].UserIndex] * ArtistFeatures[f, Ratings[i].ArtistIndex];
        }

        protected float TrainFeature(int f)
        {
            var e = Ratings.Select((r, i) => Train(i, f)).Sum() / Ratings.Count;
            return (float)Math.Sqrt(e);
        }

        protected float Train(int r, int f)
        {
            var e = Ratings[r].Value - PredictRatingWithResiduals(r, f);
            var uv = UserFeatures[f, Ratings[r].UserIndex];

            UserFeatures[f, Ratings[r].UserIndex] += LearningParameters.LRate * (e * ArtistFeatures[f, Ratings[r].ArtistIndex] - LearningParameters.K * UserFeatures[f, Ratings[r].UserIndex]);
            ArtistFeatures[f, Ratings[r].ArtistIndex] += LearningParameters.LRate * (e * uv - LearningParameters.K * ArtistFeatures[f, Ratings[r].ArtistIndex]);

            return e * e;
        }

        protected abstract float PredictRatingWithResiduals(int rating, int feature);
        public abstract float PredictRating(int u, int a);
    }
}