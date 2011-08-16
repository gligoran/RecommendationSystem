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
        private static Stopwatch timer = new Stopwatch();
        private static List<string> users = null;
        private static List<string> artists = null;
        private static List<Rating> ratings = null;
        private static int featureCount = 2;
        private static float lRate = 0.001f;
        private static List<float> err = new List<float>();
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
            //Data.ReloadData(users: true, artists: true, ratings: true);

            //load preprocessed data
            timer.Start();
            users = Data.LoadUsers(@"D:\Dataset\users.rs");
            artists = Data.LoadArtists(@"D:\Dataset\artists.rs");
            ratings = Data.LoadRatings(@"D:\Dataset\ratings.rs");
            timer.Stop();
            Console.WriteLine("Data loaded in: {0}ms", timer.ElapsedMilliseconds);

            //SVD(@"D:\Dataset\ratings.tsv");

            Console.WriteLine("Done. Press Enter to exit.");
            Console.ReadLine();
        }

        static void SVD(string filename)
        {
            //init
            user = new float[featureCount, users.Count];
            artist = new float[featureCount, artists.Count];
            user.Populate<float>(0.1f);
            artist.Populate<float>(0.1f);

            string line;
            string[] sep = new string[] { "\t" };

            int u, a;
            float r;

            int count = 0;

            //MAIN LOOP - loops through features
            for (int f = 0; f < featureCount; f++)
            {
                TextReader reader = new StreamReader(filename);
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(sep, StringSplitOptions.None);

                    u = users.IndexOf(parts[0]);
                    a = artists.IndexOf(parts[2]);
                    r = float.Parse(parts[3]);

                    Train(u, a, f, r);

                    count++;
                    if (count % 10000 == 0)
                        Console.WriteLine("at {0}", count);
                }
            }
        }

        static void Train(int u, int a, int f, float r)
        {
            float err = lRate * (r - PredictRating(u, a));
            user[f, u] += err * artist[f, a];
            artist[f, u] += err * user[f, u];
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
