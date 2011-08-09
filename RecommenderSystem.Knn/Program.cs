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
            int t = 0;
            do
            {
                Console.Write("Enter number of users to load (0 for all): ");
            } while (!int.TryParse(Console.ReadLine(), out t));

            Stopwatch timer = new Stopwatch();
            timer.Start();
            var users = Data.LoadData<RatingUser>(@"D:\Dataset\no-mbid.tsv", t).ToList();
            timer.Stop();

            /*var me = users.Where(u => u.UserId == "cb732aa2abb82e9527716dc9f083110b22265380").First();
            var meIndex = users.IndexOf(me);*/

            Console.WriteLine("{0} users loaded in {1}ms.", users.Count(), timer.ElapsedMilliseconds);

            /*double max = 0.0, c;
            timer.Restart();
            for (int i = 0; i < users.Count(); i++)
            {
                for (int j = i + 1; j < users.Count(); j++)
                {
                    c = users[i].CosineSimliarity(users[j]);
                    //if (c == 1)
                    //    Console.WriteLine("[{0}, {1}] = {2}", i, j, c);

                    if (c > max)
                        max = c;
                }
            }
            timer.Stop();*/

            Console.Write("Enter user index: ");
            while (int.TryParse(Console.ReadLine(), out t))
            {
                double max = double.MinValue, c;
                int index = -1;
                var a = users[t];
                timer.Restart();
                for (int i = 0; i < users.Count(); i++)
                {
                    if (i == t)
                        continue;

                    c = a.PearsonSimliarity(users[i]);
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
        }
    }
}
