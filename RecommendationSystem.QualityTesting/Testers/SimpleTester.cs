using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Simple.AverageRating;
using RecommendationSystem.Simple.MedianRating;
using RecommendationSystem.Simple.MostCommonRating;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting.Testers
{
    public class SimpleTester : TesterBase
    {
        public List<IUser> TrainUsers { get; set; }
        public List<IArtist> Artists { get; set; }
        public List<IRating> TrainRatings { get; set; }
        public List<IUser> TestUsers { get; set; }

        public SimpleTester(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings, List<IUser> testUsers)
        {
            TrainUsers = trainUsers;
            Artists = artists;
            TrainRatings = trainRatings;
            TestUsers = testUsers;

            TestName = "Simple";
        }

        public override void Test()
        {
            base.Test();
            
            try
            {
            var rss = new[] {"ar", "mr", "mcr"};
            Parallel.ForEach(rss, rs =>
                {
                    switch (rs)
                    {
                        case "ar":
                            var arrs = new AverageRatingRecommendationSystem();
                            var arModel = arrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                            var rv = CompleteTestRecommendationSystem(arrs, TestUsers, arModel, Artists);
                            Write(string.Format("AverageRating: {0}", rv));
                            break;
                        case "mr":
                            var mrrs = new MedianRatingRecommendationSystem();
                            var mrModel = mrrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                            rv = CompleteTestRecommendationSystem(mrrs, TestUsers, mrModel, Artists);
                            Write(string.Format("MedianRating: {0}", rv));
                            break;
                        case "mcr":
                            var mcrrs = new MostCommonRatingRecommendationSystem();
                            var mcrModel = mcrrs.Trainer.TrainModel(TrainUsers, Artists, TrainRatings);
                            rv = CompleteTestRecommendationSystem(mcrrs, TestUsers, mcrModel, Artists);
                            Write(string.Format("MostCommonRating: {0}", rv));
                            break;
                    }
                });
            }
            catch (Exception e)
            {
                Write(string.Format("{0}{1}{1}{2}", e, Environment.NewLine, e.Message));
            }
        }

        #region CompleteTestRecommendationSystem
        private RmseAndVariance CompleteTestRecommendationSystem<TModel, TUser>(IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs, IEnumerable<TUser> testUsers, TModel model, List<IArtist> artists)
            where TModel : IModel
            where TUser : IUser
        {
            var rmseList = (from user in testUsers
                            let userError = user.Ratings.Select(rating => GerPredictionError(rs, model, rating, user, artists)).Select(error => error * error).Sum()
                            select (float)Math.Sqrt(userError / user.Ratings.Count)).ToList();

            var rv = new RmseAndVariance(rmseList);
            return rv;
        }
        #endregion

        #region GerPredictionError
        private static float GerPredictionError<TModel, TUser>(IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs, TModel model, IRating rating, TUser user, List<IArtist> artists)
            where TModel : IModel
            where TUser : IUser
        {
            var originalRatings = user.Ratings;
            user.Ratings = user.Ratings.Where(r => r != rating).ToList();

            var error = rating.Value - rs.Recommender.PredictRatingForArtist(user, model, artists, rating.ArtistIndex);

            user.Ratings = originalRatings;
            return error;
        }
        #endregion
    }
}