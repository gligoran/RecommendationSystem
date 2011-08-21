using System;

namespace RecommendationSystem.MatrixFactorization
{
    public static class Program
    {
        //public static Stopwatch Timer = new Stopwatch();
        //private const string me = "cb732aa2abb82e9527716dc9f083110b22265380";

        private static void Main()
        {
            //TODO: when predictors are implemented, reeanble this
            //load preprocessed data
            /*Timer.Start();
            List<string> users,
                         artists;
            UserProvider.Load(@"D:\Dataset\users.rs", out users);
            ArtistProvider.Load(@"D:\Dataset\artists.rs", out artists);
            var ratings = RatingProvider.Load(@"D:\Dataset\ratings.rs");
            Timer.Stop();
            Console.WriteLine("Data loaded in: {0}ms", Timer.ElapsedMilliseconds);

            //train
            var svd = new BasicSvdTrainer(ratings, users, artists);
            Timer.Start();
            var model = svd.TrainModel(new TrainingParameters(100, epochLimit: 120));
            Timer.Stop();
            Console.WriteLine("SVD completed in: {0}ms", Timer.ElapsedMilliseconds);

            //calculate recommendations
            var predictor = new BasicSvdPredictor(users, artists);

            var rValues = new Dictionary<string, float>();
            for (var i = 0; i < artists.Count; i++)
                rValues.Add(artists[i], predictor.PredictRating(model, me, i));

            Console.WriteLine("me vs. lp: {0}", predictor.PredictRating(model, me, "linkin park"));

            var recs = rValues.ToList();
            recs = recs.OrderByDescending(r => r.Value).ToList();
            for (var i = 0; i < 10; i++)
                Console.WriteLine("- {0} ({1})", recs[i].Key, recs[i].Value);*/

            Console.ReadLine();
        }
    }
}