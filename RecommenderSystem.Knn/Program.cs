using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace RecommenderSystem.Knn
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var users = Data.LoadData(@"D:\Dataset\no-mbid.tsv").ToList<User>();
            timer.Stop();

            Console.WriteLine("{0} users loaded in {1}ms.", users.Count(), timer.ElapsedMilliseconds);

            /*int count = 0;
            foreach (var user in users)
            {
                count++;
                Console.WriteLine("[{0}] {1} {2}, ", count, user.UserId, user.TotalPlays);
            }*/

            int max = 0;

            timer.Restart();
            for (int i = 0; i < users.Count(); i++)
            {
                for (int j = i + 1; j < users.Count(); j++)
                {
                    int c = users[i].CompareTo(users[j]);
                    /*if (c > 20)
                        Console.WriteLine("[{0}, {1}] = {2}", i, j, c);*/

                    if (c > max)
                        max = c;
                }
            }
            timer.Stop();

            Console.WriteLine("Max of {0} found in {1}ms.", max, timer.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
