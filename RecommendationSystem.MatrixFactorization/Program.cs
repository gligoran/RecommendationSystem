using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization
{
    class Program
    {
        public static Stopwatch Timer = new Stopwatch();

        static void Main()
        {
            //load preprocessed data
            Timer.Start();
            var users = Data.LoadData(@"D:\Dataset\users.rs");
            var artists = Data.LoadData(@"D:\Dataset\artists.rs");
            var ratings = Data.LoadRatings(@"D:\Dataset\ratings.rs");
            Timer.Stop();
            Console.WriteLine("Data loaded in: {0}ms", Timer.ElapsedMilliseconds);

            //train
            var svd = new BasicSvdTrainer(ratings, users, artists);
            Timer.Start();
            var model = svd.TrainModel(TrainingParameters.DefaultTrainingParameters);
            Timer.Stop();
            Console.WriteLine("SVD completed in: {0}ms", Timer.ElapsedMilliseconds);

            //calculate recommendations
            var predictor = new BasicSvdPredictor(users, artists);

            var me = users.BinarySearch("cb732aa2abb82e9527716dc9f083110b22265380");
            var rValues = new Dictionary<string, float>();
            for (var i = 0; i < artists.Count; i++)
                rValues.Add(artists[i], predictor.PredictRating(model, me, i));

            var lp = artists.BinarySearch("linkin park");
            Console.WriteLine("me vs. lp: {0}", predictor.PredictRating(model, me, lp));

            var recs = rValues.ToList();
            recs = recs.OrderByDescending(r => r.Value).ToList();
            for (var i = 0; i < 10; i++)
                Console.WriteLine("- {0} ({1})", recs[i].Key, recs[i].Value);

            Console.ReadLine();
        }
    }
}
