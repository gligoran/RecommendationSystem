using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RecommendationSystem.Entities;
using RecommendationSystem.SvdBoostedKnn;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class SvdBoostedKnnTester<TSvdBoostedKnnModel> : TesterBase
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        #region Fields
        private CryptoRandom rng = new CryptoRandom();
        private int writeFrequency;
        #endregion

        #region Properties
        public ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel> RecommendationSystem { get; set; }
        public TSvdBoostedKnnModel Model { get; set; }
        public List<IUser> TestUsers { get; set; }
        public List<IRating> TestRatings { get; set; }
        public List<IArtist> Artists { get; set; }
        public int NumberOfTests { get; set; }
        #endregion

        public SvdBoostedKnnTester(string testName, ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel> recommendationSystem, TSvdBoostedKnnModel model, List<IUser> testUsers, List<IRating> testRatings, List<IArtist> artists, int numberOfTests)
        {
            TestName = testName;
            RecommendationSystem = recommendationSystem;
            Model = model;
            TestUsers = testUsers;
            TestRatings = testRatings;
            Artists = artists;
            NumberOfTests = numberOfTests;
        }

        #region Test
        public override void Test()
        {
            base.Test();

            writeFrequency = (int)Math.Ceiling(NumberOfTests / 100d);

            Timer.Restart();
            RmseBiasAndVariance[] rbvsByRatings;
            var rbv = TestRecommendationSystem(out rbvsByRatings);
            Timer.Stop();
            for (var i = 0; i < rbvsByRatings.Length; i++)
                Write(string.Format(CultureInfo.InvariantCulture, "{0}\t->\tRating:{1}\t{2}.", TestName, i + 1, rbvsByRatings[i]));

            Write(string.Format(CultureInfo.InvariantCulture, "{0}\t->\tAll ratings\t{1}\t({2}).", TestName, rbv, TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds)));
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
                        var ratingIndex = rng.Next(user.Ratings.Count);
                        var rating = user.Ratings[ratingIndex];

                        var originalRatings = user.Ratings;
                        user.Ratings = user.Ratings.Where(r => r != rating).ToList();

                        var predictedRating = RecommendationSystem.Recommender.PredictRatingForArtist(user, Model, Artists, rating.ArtistIndex);
                        var error = predictedRating - rating.Value;
                        biasBC[(int)rating.Value - 1].Add(error);
                        rmseBC[(int)rating.Value - 1].Add((float)Math.Sqrt(error * error));

                        user.Ratings = originalRatings;

                        //Write(string.Format("{0}\t{1}", predictedRating, rating.Value), false);

                        if (rmseBC.Sum(bc => bc.Count) % writeFrequency == 0)
                            Write(string.Format("Test {0} at {1} ({2})", TestName, rmseBC.Sum(bc => bc.Count), DateTime.Now), toFile: false);
                    }
                });

            while (rmseBC.Sum(bc => bc.Count) < NumberOfTests)
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
    }
}