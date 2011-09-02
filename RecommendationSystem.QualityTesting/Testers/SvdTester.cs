using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Recommendation;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class SvdTester<TSvdModel> : TesterBase
        where TSvdModel : ISvdModel
    {
        public List<IUser> TestUsers { get; set; }
        public List<IRating> TestRatings { get; set; }
        public List<IArtist> Artists { get; set; }

        public IRecommendationSystem<TSvdModel, IUser, ISvdTrainer<TSvdModel>, ISvdRecommender<TSvdModel>> RecommendationSystem { get; set; }
        public TSvdModel Model { get; set; }

        public SvdTester(string testName, IRecommendationSystem<TSvdModel, IUser, ISvdTrainer<TSvdModel>, ISvdRecommender<TSvdModel>> recommendationSystem, TSvdModel model, List<IUser> testUsers, List<IRating> testRatings, List<IArtist> artists)
        {
            RecommendationSystem = recommendationSystem;
            Model = model;
            TestUsers = testUsers;
            TestRatings = testRatings;
            Artists = artists;

            TestName = testName;
        }

        public override void Test()
        {
            base.Test();

            Timer.Restart();
            RmseAndBias[] rbsByRatings;
            var rb = TestRecommendationSystem(out rbsByRatings);
            Timer.Stop();
            for (var i = 0; i < rbsByRatings.Length; i++)
                Write(string.Format("{0}\t->\tRating:{1}\t{2}.", TestName, i + 1, rbsByRatings[i]));

            Write(string.Format("{0}\t->\tAll ratings\t{1}\t({2}).", TestName, rb, TimeSpan.FromMilliseconds(Timer.ElapsedMilliseconds)));
        }

        #region CompleteTestRecommendationSystem
        private RmseAndBias TestRecommendationSystem(out RmseAndBias[] rvsByRatings)
        {
            var rmseBC = new BlockingCollection<float>[5];
            for (var i = 0; i < rmseBC.Length; i++)
                rmseBC[i] = new BlockingCollection<float>();

            var biasBC = new BlockingCollection<float>[5];
            for (var i = 0; i < biasBC.Length; i++)
                biasBC[i] = new BlockingCollection<float>();

            var percent = TestUsers.Count / 100;
            for (var i = 0; i < TestUsers.Count; i++)
            {
                var user = TestUsers[i];
                lock (user)
                {
                    if (user.Ratings.Count > 1)
                    {
                        var originalRatings = user.Ratings;
                        foreach (var rating in user.Ratings)
                        {
                            user.Ratings = originalRatings.Where(r => r != rating).ToList();
                            var predictedRating = RecommendationSystem.Recommender.PredictRatingForArtist(user, Model, Artists, rating.ArtistIndex);

                            //Write(string.Format("{0}\t{1}", Math.Round(predictedRating * 2.0, 0) / 2.0f, rating.Value), false);

                            var error = predictedRating - rating.Value;
                            biasBC[(int)rating.Value - 1].Add(error);
                            rmseBC[(int)rating.Value - 1].Add((float)Math.Sqrt(error * error));
                        }
                        user.Ratings = originalRatings;
                    }
                }

                if (i % percent == 0)
                    Write(string.Format("{0} at {1} ({2}%) with {3}", TestName, i, i / percent, GetRmseAndBias(biasBC, rmseBC)), toFile: false);
            }

            return GetRmseAndBias(out rvsByRatings, biasBC, rmseBC);
        }

        private static RmseAndBias GetRmseAndBias(BlockingCollection<float>[] biasBC, BlockingCollection<float>[] rmseBC)
        {
            var totalRmse = new List<float>();
            var totalBias = new List<float>();
            for (var i = 0; i < rmseBC.Length; i++)
            {
                totalRmse.AddRange(rmseBC[i].ToList());
                totalBias.AddRange(biasBC[i].ToList());
            }

            return new RmseAndBias(totalRmse, totalBias);
        }

        private static RmseAndBias GetRmseAndBias(out RmseAndBias[] rbsByRatings, BlockingCollection<float>[] biasBC, BlockingCollection<float>[] rmseBC)
        {
            rbsByRatings = new RmseAndBias[5];
            var totalRmse = new List<float>();
            var totalBias = new List<float>();
            for (var i = 0; i < rmseBC.Length; i++)
            {
                if (rmseBC[i].Count > 0)
                    rbsByRatings[i] = new RmseAndBias(rmseBC[i].ToList(), biasBC[i].ToList());
                else
                    rbsByRatings[i] = new RmseAndBias();

                totalRmse.AddRange(rmseBC[i].ToList());
                totalBias.AddRange(biasBC[i].ToList());
            }

            return new RmseAndBias(totalRmse, totalBias);
        }
        #endregion
    }
}