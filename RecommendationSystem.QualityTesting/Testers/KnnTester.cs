using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class KnnTester<TRecommender> : TesterBase
        where TRecommender : IRecommender<IKnnModel>
    {
        #region Fields
        private CryptoRandom rng = new CryptoRandom();
        private IRecommendationSystem<IKnnModel, IUser, ITrainer<IKnnModel, IUser>, IRecommender<IKnnModel>> rs;
        private int writeFrequency;
        #endregion

        #region Properties
        public int K { get; set; }
        public ISimilarityEstimator Sim { get; set; }
        public IRatingAggregator Ra { get; set; }
        public List<IUser> TestUsers { get; set; }
        public IKnnModel KnnModel { get; set; }
        public ITrainer<IKnnModel, IUser> Trainer { get; set; }
        public List<IArtist> Artists { get; set; }
        public int NumberOfTests { get; set; }
        #endregion

        #region Test
        public override void Test()
        {
            var recommender = (TRecommender)Activator.CreateInstance(typeof(TRecommender), new object[] {Sim, Ra, K});
            TestName = string.Format("Knn-K{0}-{1}-{2}-{3}-T{4}", K, Sim, Ra, recommender, NumberOfTests);
            writeFrequency = NumberOfTests / 100;

            InitializeResultWriter(String.Format(@"D:\Dataset\results\{0}.txt", TestName));

            try
            {
                Write(string.Format("Test {0} ({1})", TestName, DateTime.Now));
                Write("------------------------------------------------------", false);

                rs = new KnnRecommendationSystem(Trainer, recommender);
                Timer.Restart();
                var rv = TestRecommendationSystem();
                Timer.Stop();

                Write("------------------------------------------------------", false);
                Write(string.Format("RMSE({0}) = {1} ({2}ms).", TestName, rv, Timer.ElapsedMilliseconds));
            }
            catch (Exception e)
            {
                Write(string.Format("{0}{1}{1}{2}", e, Environment.NewLine, e.Message));
            }

            FileWriter.Close();
        }
        #endregion

        #region TestRecommendationSystem
        private RmseAndVariance TestRecommendationSystem()
        {
            var rmseBC = new BlockingCollection<float>();
            var x = Parallel.For(0, NumberOfTests, i =>
                {
                    var userIndex = rng.Next(TestUsers.Count);
                    var user = TestUsers[userIndex];
                    lock (user)
                    {
                        //if true we'll remove the only rating
                        if (user.Ratings.Count > 1)
                            GetError(rmseBC, user);
                    }
                });
            rmseBC.CompleteAdding();

            while (!x.IsCompleted)
            {}

            var rmseList = rmseBC.ToList();
            var rv = new RmseAndVariance(rmseList);
            return rv;
        }
        #endregion

        #region GetError
        private void GetError(BlockingCollection<float> rmseBC, IUser user)
        {
            var ratingIndex = rng.Next(user.Ratings.Count);
            var rating = user.Ratings[ratingIndex];

            var originalRatings = user.Ratings;
            user.Ratings = user.Ratings.Where(r => r != rating).ToList();

            var error = rs.Recommender.PredictRatingForArtist(user, KnnModel, Artists, rating.ArtistIndex) - rating.Value;
            rmseBC.Add((float)Math.Sqrt(error * error));

            user.Ratings = originalRatings;

            if (rmseBC.Count % writeFrequency == 0)
                Write(string.Format("Test {0} at {1} in {2}ms with RMSE {3} ({4})", TestName, rmseBC.Count, Timer.ElapsedMilliseconds, rmseBC.Average(), DateTime.Now));
        }
        #endregion

        #region GetCompleteError
        private void GetCompleteError(BlockingCollection<float> rmseList, IUser user)
        {
            foreach (var rating in user.Ratings)
            {
                var originalRatings = user.Ratings;
                user.Ratings = user.Ratings.Where(r => r != rating).ToList();

                var error = rating.Value - rs.Recommender.PredictRatingForArtist(user, KnnModel, Artists, rating.ArtistIndex);
                rmseList.Add((float)Math.Sqrt(error * error));

                user.Ratings = originalRatings;

                if (rmseList.Count % writeFrequency == 0)
                    Write(string.Format("Test {0} at {1} in {2}ms with RMSE {3} ({4})", TestName, rmseList.Count, Timer.ElapsedMilliseconds, rmseList.Average(), DateTime.Now));
            }
        }
        #endregion
    }
}