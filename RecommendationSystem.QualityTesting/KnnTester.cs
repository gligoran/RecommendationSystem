using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting
{
    public class KnnTester<TRecommender>
        where TRecommender : IRecommender<IKnnModel>
    {
        private readonly Stopwatch timer = new Stopwatch();
        private CryptoRandom rng = new CryptoRandom();
        private TextWriter fileWriter;

        public int K { get; set; }
        public ISimilarityEstimator Sim { get; set; }
        public IRatingAggregator Ra { get; set; }
        public List<IUser> TestUsers { get; set; }
        public IKnnModel KnnModel { get; set; }
        public ITrainer<IKnnModel, IUser> Trainer { get; set; }
        public List<IArtist> Artists { get; set; }
        public int NumberOfTests { get; set; }
        public string TestName { get; set; }

        public KnnTester()
        {
        }

        #region KnnTest
        public void KnnTest()
        {
            var recommender = (TRecommender)Activator.CreateInstance(typeof(TRecommender), new object[] { Sim, Ra, K });
            TestName = string.Format("Knn-K{0}-{1}-{2}-{3}-T{4}", K, Sim, Ra, recommender, NumberOfTests);

            InitializeResultWriter(String.Format(@"D:\Dataset\results\{0}.txt", TestName));
            Write(string.Format("Test {0} ({1})", TestName, DateTime.Now));
            Write("------------------------------------------------------", false);

            var rs = new KnnRecommendationSystem(Trainer, recommender);
            timer.Restart();
            var rv = TestRecommendationSystem(rs);
            timer.Stop();

            Write("------------------------------------------------------", false);
            Write(string.Format("RMSE({0}) = {1} ({2}ms).", TestName, rv, timer.ElapsedMilliseconds));

            fileWriter.Close();
        }
        #endregion

        #region TestRecommendationSystem
        private RmseAndVariance TestRecommendationSystem<TModel, TUser>(IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs)
            where TModel : IModel
            where TUser : IUser
        {
            var onePrecent = NumberOfTests / 100;
            var rmseList = new List<float>();
            while (rmseList.Count < NumberOfTests)
            {
                var userIndex = rng.Next(TestUsers.Count);
                var user = TestUsers[userIndex];
                lock (user)
                {
                    //if true we'll remove the only rating
                    if (user.Ratings.Count < 2)
                        continue;

                    var ratingIndex = rng.Next(user.Ratings.Count);
                    var rating = user.Ratings[ratingIndex];

                    var error = GerPredictionError(rs, rating, user);

                    rmseList.Add((float)Math.Sqrt(error * error));
                }

                if (rmseList.Count % onePrecent != 0)
                    continue;

                Write(string.Format("Test {0} at {1} in {2}ms with RMSE {3} ({4})", TestName, rmseList.Count, timer.ElapsedMilliseconds, rmseList.Sum() / rmseList.Count, DateTime.Now));
            }

            var rv = new RmseAndVariance(rmseList);
            return rv;
        }
        #endregion

        #region GerPredictionError
        private float GerPredictionError<TModel, TUser>(IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs, IRating rating, IUser user)
            where TModel : IModel
            where TUser : IUser
        {
            var originalRatings = user.Ratings;
            user.Ratings = user.Ratings.Where(r => r != rating).ToList();

            var error = rating.Value - rs.Recommender.PredictRatingForArtist(user, (TModel)KnnModel, Artists, rating.ArtistIndex);

            user.Ratings = originalRatings;
            return error;
        }
        #endregion

        #region SavingResults
        private void InitializeResultWriter(string filename)
        {
            var dir = Path.GetDirectoryName(filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            fileWriter = new StreamWriter(filename);
        }

        private void Write(string text, bool toConsole = true, bool toFile = true)
        {
            if (toConsole)
                Console.WriteLine(text);

            if (!toFile)
                return;

            fileWriter.WriteLine(text);
            fileWriter.Flush();
        }
        #endregion
    }
}