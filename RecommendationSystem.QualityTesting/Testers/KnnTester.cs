using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Users;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class KnnTester<TRecommender> : TesterBase
        where TRecommender : IRecommender<ISimpleKnnModel>
    {
        #region Fields
        private CryptoRandom rng = new CryptoRandom();
        private IRecommendationSystem<ISimpleKnnModel, IUser, ITrainer<ISimpleKnnModel>, IRecommender<ISimpleKnnModel>> rs;
        private int writeFrequency;
        #endregion

        #region Properties
        public int K { get; set; }
        public ISimilarityEstimator<ISimpleKnnUser> Sim { get; set; }
        public IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> Rg { get; set; }
        public List<IUser> TestUsers { get; set; }
        public ISimpleKnnModel SimpleKnnModel { get; set; }
        public ITrainer<ISimpleKnnModel> Trainer { get; set; }
        public List<IArtist> Artists { get; set; }
        public int NumberOfTests { get; set; }
        #endregion

        #region Test
        public override void Test()
        {
            var recommender = (TRecommender)Activator.CreateInstance(typeof(TRecommender), new object[] {Sim, Rg, K});
            TestName = string.Format("Knn-K{0}-{1}-{2}-{3}-T{4}", K, Sim, Rg, recommender, NumberOfTests);
            writeFrequency = (int)Math.Ceiling(NumberOfTests / 100d);

            base.Test();

            try
            {
                Write(string.Format("Test {0} ({1})", TestName, DateTime.Now));
                Write("------------------------------------------------------", false);

                rs = new SimpleKnnRecommendationSystem(Trainer, recommender);
                Timer.Restart();
                RmseBiasAndVariance[] rbvsByRatings;
                var rbv = TestRecommendationSystem(out rbvsByRatings);
                Timer.Stop();

                Write("------------------------------------------------------", false);
                for (var i = 0; i < rbvsByRatings.Length; i++)
                    Write(string.Format(CultureInfo.InvariantCulture, "{0}\t->\tRating:{1}\t{2}.", TestName, i + 1, rbvsByRatings[i]));

                Write(string.Format(CultureInfo.InvariantCulture, "{0}\t->\tAll ratings\t{1}\t({2}).", TestName, rbv, TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds)));
            }
            catch (Exception e)
            {
                Write(string.Format(CultureInfo.InvariantCulture, "{0}{1}{1}{2}", e, Environment.NewLine, e.Message));
            }

            FileWriter.Close();
        }
        #endregion

        #region TestRecommendationSystem
        private RmseBiasAndVariance TestRecommendationSystem(out RmseBiasAndVariance[] rbvsByRatings)
        {
            var rmseBC = new BlockingCollection<float>[5];
            for (var i = 0; i < rmseBC.Length; i++)
                rmseBC[i] = new BlockingCollection<float>();

            var biasBC = new BlockingCollection<float>[5];
            for (var i = 0; i < biasBC.Length; i++)
                biasBC[i] = new BlockingCollection<float>();

            Parallel.For(0, NumberOfTests, i =>
                {
                    IUser user;
                    do
                    {
                        user = TestUsers[rng.Next(TestUsers.Count)];
                    } while (user.Ratings.Count < 2);

                    lock (user)
                    {
                        GetError(rmseBC, biasBC, user);
                    }
                });

            while (rmseBC.Sum(bc => bc.Count) != NumberOfTests)
            {}

            rbvsByRatings = new RmseBiasAndVariance[5];
            var totalRmse = new List<float>();
            var totalBias = new List<float>();
            for (var i = 0; i < rmseBC.Length; i++)
            {
                if (rmseBC[i].Count > 0)
                    rbvsByRatings[i] = new RmseBiasAndVariance(rmseBC[i].ToList(), biasBC[i].ToList());
                else
                    rbvsByRatings[i] = new RmseBiasAndVariance();

                totalRmse.AddRange(rmseBC[i].ToList());
                totalBias.AddRange(biasBC[i].ToList());
            }
            return new RmseBiasAndVariance(totalRmse, totalBias);
        }
        #endregion

        #region GetError
        private void GetError(BlockingCollection<float>[] rmseBC, BlockingCollection<float>[] biasBC, IUser user)
        {
            var ratingIndex = rng.Next(user.Ratings.Count);
            var rating = user.Ratings[ratingIndex];

            var originalRatings = user.Ratings;
            user.Ratings = user.Ratings.Where(r => r != rating).ToList();

            var predictedRating = rs.Recommender.PredictRatingForArtist(user, SimpleKnnModel, Artists, rating.ArtistIndex);
            var error = predictedRating - rating.Value;
            biasBC[(int)rating.Value - 1].Add(error);
            rmseBC[(int)rating.Value - 1].Add((float)Math.Sqrt(error * error));

            user.Ratings = originalRatings;

            Write(string.Format("{0}\t{1}", predictedRating, rating.Value), false);

            if (rmseBC.Sum(bc => bc.Count) % writeFrequency == 0)
                Write(string.Format("Test {0} at {1} ({2})", TestName, rmseBC.Sum(bc => bc.Count), DateTime.Now), toFile: false);
        }
        #endregion

        #region GetCompleteError
        private void GetCompleteError(BlockingCollection<float> rmseList, IUser user)
        {
            foreach (var rating in user.Ratings)
            {
                var originalRatings = user.Ratings;
                user.Ratings = user.Ratings.Where(r => r != rating).ToList();

                var error = rating.Value - rs.Recommender.PredictRatingForArtist(user, SimpleKnnModel, Artists, rating.ArtistIndex);
                rmseList.Add((float)Math.Sqrt(error * error));

                user.Ratings = originalRatings;

                if (rmseList.Count % writeFrequency == 0)
                    Write(string.Format("Test {0} at {1} in {2}ms with RMSE {3} ({4})", TestName, rmseList.Count, Timer.ElapsedMilliseconds, rmseList.Average(), DateTime.Now));
            }
        }
        #endregion
    }
}