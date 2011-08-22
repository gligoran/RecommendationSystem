using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Data;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Simple.AverageRating;
using RecommendationSystem.Simple.MedianRating;
using RecommendationSystem.Simple.MostCommonRating;
using RecommendationSystem.Training;

namespace RecommendationSystem.QualityTesting
{
    internal static class Program
    {
        public static void Main()
        {
            //load train data
            var trainRatings = RatingProvider.Load(DataFiles.TrainEqualFerquencyFiveScaleRatings);
            var trainUsers = UserProvider.Load(DataFiles.TrainUsers);
            var artists = ArtistProvider.Load(DataFiles.Artists);

            //initialize rs'
            var ar = new AverageRatingRecommendationSystem();
            var mr = new MedianRatingRecommendationSystem();
            var mcr = new MostCommonRatingRecommendationSystem();

            //train models
            var arModel = ar.Trainer.TrainModel(trainUsers, artists, trainRatings);
            var mrModel = mr.Trainer.TrainModel(trainUsers, artists, trainRatings);
            var mcrModel = mcr.Trainer.TrainModel(trainUsers, artists, trainRatings);

            //load test data
            var testRatings = RatingProvider.Load(DataFiles.TestEqualFerquencyFiveScaleRatings);
            var testUsers = UserProvider.Load(DataFiles.TestUsers);
            testUsers.PopulateWithRatings(testRatings);

            var rmse = GetRmse(artists, testUsers, arModel, ar);
            Console.WriteLine("AR rmse = {0}", rmse);

            rmse = GetRmse(artists, testUsers, mrModel, mr);
            Console.WriteLine("MR rmse = {0}", rmse);

            rmse = GetRmse(artists, testUsers, mcrModel, mcr);
            Console.WriteLine("MCR rmse = {0}", rmse);

            Console.ReadLine();
        }

        private static float GetRmse<TModel, TUser>(List<IArtist> artists, List<TUser> testUsers, TModel model, IRecommendationSystem<TModel, TUser, ITrainer<TModel, TUser>, IRecommender<TModel>> rs)
            where TModel : IModel
            where TUser : IUser
        {
            var rmse = 0.0f;
            foreach (var user in testUsers)
            {
                var recommendations = rs.Recommender.GenerateRecommendations(user, model, artists);
                float userRmse = 0;
                foreach (var recommendation in recommendations)
                {
                    foreach (var rating in user.Ratings)
                        userRmse += GetSquaredError(rating, recommendation, artists);
                }
                userRmse /= user.Ratings.Count();
                rmse += (float)Math.Sqrt(userRmse);
            }
            return rmse / testUsers.Count;
        }

        private static float GetSquaredError(IRating rating, IRecommendation recommendation, List<IArtist> artists)
        {
            if (artists[rating.ArtistIndex] != recommendation.Artist)
                return 0.0f;

            var r = rating.Value - recommendation.Value;
            return r * r;
        }
    }
}