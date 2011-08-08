using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecommenderSystem.Data;

namespace RecommenderSystem.Knn
{
    class Program
    {
        static void Main(string[] args)
        {
            var users = Manager.DeserializeData<KnnUser>(@"D:\dataset\10ku.xml");
            foreach (var user in users)
            {
                Console.Write("{0}, ", user.PlayCounts.TotalPlays);
            }
        }

        /*static double Similarity(User a, User b)
        {
            foreach (var pc in a.PlayCounts)
            {
                if (b.PlayCounts.Contains(pc))
                {
                }

            }

            return 0.0;
        }*/
    }
}
