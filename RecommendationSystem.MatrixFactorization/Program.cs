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
        private const int featureCount = 25;
        private const float lRate = 0.001f;
        private const float K = 0.02f;
        private static float[] residual;
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
            residual = new float[ratings.Count];
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
                while (rmseDiff > 0.0001f || count > 100)
                {
                    rmsePrev = rmse;
                    rmse = TrainingSession(f);
                    rmseDiff = rmsePrev - rmse;

                    count++;
                    Console.WriteLine("Pass {0}/{1}:\trmse = {2}\trmseDiff = {3}", f, count, rmse, rmseDiff);
                }

                //cache residuals
                for (int i = 0; i < ratings.Count; i++)
                {
                    residual[i] += user[f, ratings[i].UserIndex] * artist[f, ratings[i].ArtistIndex];
                }
            }
        }

        private static float TrainingSession(int f)
        {
            float e = 0.0f;
            for (int i = 0; i < ratings.Count; i++)
            {
                e += Train(ratings[i].UserIndex,
                           ratings[i].ArtistIndex,
                           f,
                           ratings[i].Value,
                           residual[i]);
            }

            e /= ratings.Count;
            return (float)Math.Sqrt(e);
        }

        static float Train(int u, int a, int f, float r, float residual)
        {
            float e = r - (residual + user[f, u] * artist[f, a]);
            float uv = user[f, u];

            user[f, u] += lRate * (e * artist[f, a] - K * user[f, u]);
            artist[f, a] += lRate * (e * uv - K * artist[f, a]);

            return e * e;
        }

        static float PredictRating(int u, int a)
        {
            float rating = 0.0f;
            for (int i = 0; i < featureCount; i++)
                rating += user[i, u] * artist[i, a];

            return rating;
        }
    }
}
