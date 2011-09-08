using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Naive.AverageRating;
using RecommendationSystem.Naive.MedianRating;
using RecommendationSystem.Naive.MostCommonRating;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class NaiveTester : TesterBase
    {
        public List<IUser> TrainUsers { get; set; }
        public List<IArtist> Artists { get; set; }
        public List<IRating> TrainRatings { get; set; }
        public List<IUser> TestUsers { get; set; }

        public NaiveTester(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings, List<IUser> testUsers)
        {
            TrainUsers = trainUsers;
            Artists = artists;
            TrainRatings = trainRatings;
            TestUsers = testUsers;

            TestName = "Naive";
        }

        public override void Test()
        {
            base.Test();

            try
            {
                var rss = new[] {"ar", "mr", "mcr"};
                var test = Parallel.ForEach(rss, rs =>
                    {
                        switch (rs)
                        {
                            case "ar":
                                var arrs = new AverageRatingRecommendationSystem();
                                var arModel = arrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                                var rv = TestRecommendationSystem(arrs, TestUsers, arModel, Artists);
                                Write(string.Format("AverageRating: {0}", rv));
                                break;
                            case "mr":
                                var mrrs = new MedianRatingRecommendationSystem();
                                var mrModel = mrrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                                rv = TestRecommendationSystem(mrrs, TestUsers, mrModel, Artists);
                                Write(string.Format("MedianRating: {0}", rv));
                                break;
                            case "mcr":
                                var mcrrs = new MostCommonRatingRecommendationSystem();
                                var mcrModel = mcrrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                                rv = TestRecommendationSystem(mcrrs, TestUsers, mcrModel, Artists);
                                Write(string.Format("MostCommonRating: {0}", rv));
                                break;
                        }
                    });
                while (!test.IsCompleted)
                {}
            }
            catch (Exception e)
            {
                Write(string.Format("{0}{1}{1}{2}", e, Environment.NewLine, e.Message));
            }
        }

        #region CompleteTestRecommendationSystem
        private RmseAndBias TestRecommendationSystem<TModel, TUser>(IRecommendationSystem<TModel, TUser, ITrainer<TModel>, IRecommender<TModel>> rs, IEnumerable<TUser> testUsers, TModel model, List<IArtist> artists)
            where TModel : IModel
            where TUser : IUser
        {
            var rmseList = new List<float>();
            var biasList = new List<float>();
            foreach (var user in TestUsers)
            {
                lock (user)
                {
                    var originalRatings = user.Ratings;
                    foreach (var rating in user.Ratings)
                    {
                        user.Ratings = originalRatings.Where(r => r != rating).ToList();
                        var predictedRating = rs.Recommender.PredictRatingForArtist(user, model, artists, rating.ArtistIndex);
                        var error = predictedRating - rating.Value;
                        biasList.Add(error);
                        rmseList.Add((float)Math.Sqrt(error * error));
                    }
                    user.Ratings = originalRatings;
                }
            }

            return new RmseAndBias(rmseList, biasList);
        }
        #endregion
    }
}