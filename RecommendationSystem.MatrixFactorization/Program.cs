using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace RecommendationSystem.MatrixFactorization
{
    class Program
    {
        public static Stopwatch timer = new Stopwatch();
        private static List<string> users = null;
        private static List<string> artists = null;
        private static List<Rating> ratings = null;
        private static int featureCount = 25;
        private static float lRate = 0.001f;
        private static float K = 0.02f;
        //private static List<float> err = new List<float>();
        private static float[,] user;
        private static float[,] artist;

        static void Main(string[] args)
        {
            /*//get pefered number of features
            do
            {
                Console.Write("Enter number of features (must be greater then 0: ");
            } while (!int.TryParse(Console.ReadLine(), out featureCount) && featureCount >= 1);

            //get learning rate
            do
            {
                Console.Write("Enter learning rate (must be greater then 0): ");
            } while (!float.TryParse(Console.ReadLine(), out lRate) && lRate > 0);*/

            //preprocess data
            /*timer.Start();
            Data.ReloadData(users: true, artists: true, ratings: true);
            timer.Stop();
            Console.WriteLine("Reload complete ({0}ms).", timer.ElapsedMilliseconds);*/

            /*users = Data.LoadUsers(@"D:\Dataset\users.rs");
            var playCounts = Data.LoadRatings(@"D:\Dataset\playcounts.rs");
            timer.Start();
            ratings = Data.ConvertTo1To5Ratings(playCounts, users.Count);
            timer.Stop();
            Console.WriteLine("Converted in {0}ms.", timer.ElapsedMilliseconds);
            Data.SaveRatings(ratings, @"D:\Dataset\ratings.rs");*/

            //load preprocessed data
            timer.Start();
            users = Data.LoadUsers(@"D:\Dataset\users.rs");
            artists = Data.LoadArtists(@"D:\Dataset\artists.rs");
            ratings = Data.LoadRatings(@"D:\Dataset\ratings.rs");
            timer.Stop();
            Console.WriteLine("Data loaded in: {0}ms", timer.ElapsedMilliseconds);

            //train
            timer.Start();
            SVD(@"D:\Dataset\ratings.tsv");
            timer.Stop();
            Console.WriteLine("SVD completed in: {0}ms", timer.ElapsedMilliseconds);

            //calculate recommendations
            var me = users.BinarySearch("cb732aa2abb82e9527716dc9f083110b22265380");
            var rValues = new Dictionary<string, float>();
            for (int i = 0; i < artists.Count; i++)
                rValues.Add(artists[i], PredictRating(me, i));

            var lp = artists.BinarySearch("linkin park");
            Console.WriteLine("me vs. lp: {0}", PredictRating(me, lp));

            var recs = rValues.ToList();
            recs = recs.OrderByDescending(r => r.Value).ToList();
            for (int i = 0; i < 10; i++)
                Console.WriteLine("- {0} ({1})", recs[i].Key, recs[i].Value);

            Console.ReadLine();
        }

        static void SVD(string filename)
        {
            //init
            user = new float[featureCount, users.Count];
            artist = new float[featureCount, artists.Count];
            user.Populate<float>(0.1f);
            artist.Populate<float>(0.1f);

            int count;
            float rmsePrev = float.MaxValue;
            float rmse = float.MaxValue;
            float rmseDiff = float.MaxValue;

            //MAIN LOOP - loops through features
            for (int f = 0; f < featureCount; f++)
            {
                count = 0;
                rmseDiff = float.MaxValue;

                Console.WriteLine("Training feature {0}", f);

                //INNER LOOP - converges features
                while (rmseDiff > 0.0001f)
                {
                    rmsePrev = rmse;
                    rmse = TrainingSession(f);
                    rmseDiff = rmsePrev - rmse;

                    count++;
                    Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseDiff = {3}", f, count, rmse, rmseDiff);
                }
            }
        }

        private static float TrainingSession(int f)
        {
            float e = 0.0f;
            foreach (var rating in ratings)
            {
                e += Train(rating.UserIndex,
                           rating.ArtistIndex,
                           f,
                           rating.Value);

#if DEBUG
                if (float.IsNaN(e))
                    Console.WriteLine("Error too big");
#endif
            }

            e /= ratings.Count;
            //return e; //MSE
            return (float)Math.Sqrt(e); //RMSE
        }

        static float Train(int u, int a, int f, float r)
        {
            float e = r - PredictRating(u, a);
            float uv = user[f, u];

#if DEBUG
            if (float.IsNaN(uv))
                Console.WriteLine("NaN: uv (f: {0}, u: {1}, a: {2})", f, u, a);

            float tmp;
            tmp = user[f, u] + lRate * (e * artist[f, a] - K * user[f, u]);
            if (float.IsNaN(tmp))
                Console.WriteLine("NaN: user[{0}, {1}]", f, u);
            user[f, u] = tmp;

            tmp = artist[f, a] + lRate * (e * uv - K * artist[f, a]);
            if (float.IsNaN(tmp))
                Console.WriteLine("NaN: artist[{0}, {1}]", f, a);
            artist[f, a] = tmp;

            if (float.IsNaN(user[f, u]))
                Console.WriteLine("NaN: user[{0}, {1}]", f, u);

            if (float.IsNaN(artist[f, a]))
                Console.WriteLine("NaN: artist[{0}, {1}]", f, a);
#else
            user[f, u] += lRate * (e * artist[f, a] - K * user[f, u]);
            artist[f, a] += lRate * (e * uv - K * artist[f, a]);
#endif

            return e * e;
        }

        static float PredictRating(int u, int a)
        {
            float rating = 0.0f;
            for (int i = 0; i < featureCount; i++)
                rating += user[i, u] * artist[i, a];

#if DEBUG
            if (float.IsNaN(rating))
                Console.WriteLine("NaN: u: {0}, a: {1}]", u, a);
#endif

            return rating;
        }
    }
}
