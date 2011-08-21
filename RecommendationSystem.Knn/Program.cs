using System;
using System.Diagnostics;
using System.Linq;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn
{
    public static class Program
    {
        private static readonly Stopwatch timer = new Stopwatch();

        private static void Main()
        {
            int t;
            do
            {
                Console.Write("Enter number of users to load (0 for all): ");
            } while (!int.TryParse(Console.ReadLine(), out t));

            if (t == 0)
                t = int.MaxValue;

            // get users
            timer.Start();
            var users = Manager.LoadData<PlayCountShareUser>(@"D:\Dataset\data-with-mbids.tsv", t).ToList<User>();
            timer.Stop();
            Console.WriteLine("{0} users loaded in {1}ms.", users.Count(), timer.ElapsedMilliseconds);

            if (t > 285392)
            {
                var me = users.Where(u => u.UserId == "cb732aa2abb82e9527716dc9f083110b22265380").First();
                //Console.WriteLine(me);
                //SimilaritiesTester(users, new CosineSimilarityEstimator());
                //var meIndex = users.IndexOf(me);

                Manager.CalculateKNearestNeighboursForUser(me, users, new PearsonSimilarityEstimator());
                //foreach (var n in me.Neighbours)
                //    Console.WriteLine(n);

                var rs = Manager.GetRecommendations(me, new SimpleAverageRatingAggregator(), 24);
                Console.WriteLine("Simple average rating aggregation:");
                foreach (var r in rs)
                    Console.WriteLine("- {0}", r);

                rs = Manager.GetRecommendations(me, new WeightedSumRatingAggregator(), 24);
                Console.WriteLine("Weighted sum rating aggregation:");
                foreach (var r in rs)
                    Console.WriteLine("- {0}", r);

                rs = Manager.GetRecommendations(me, new AdjustedWeightedSumRatingAggregator(), 24);
                Console.WriteLine("Adjusted weighted sum rating aggregation:");
                foreach (var r in rs)
                    Console.WriteLine("- {0}", r);
            }

            Console.ReadLine();
        }

        /*private static void SimilaritiesTester(List<User> users, ISimilarityEstimator similarityEstimator)
        {
            int t;
            Console.Write("Enter user index: ");
            while (int.TryParse(Console.ReadLine(), out t))
            {
                float max = float.MinValue;
                int index = -1;
                var a = users[t];
                timer.Restart();
                for (int i = 0; i < users.Count(); i++)
                {
                    if (i == t)
                        continue;

                    float c = similarityEstimator.Similarity(a, users[i]);
                    //if (c == 1)
                    //    Console.WriteLine("[{0}, {1}] = {2}", i, j, c);

                    if (c > max)
                    {
                        max = c;
                        index = i;
                    }
                }
                timer.Stop();

                Console.WriteLine("Max of {0} at {1} found in {2}ms.", max, index, timer.ElapsedMilliseconds);
                Console.Write("Enter next user index: ");
            }
        }*/
    }
}