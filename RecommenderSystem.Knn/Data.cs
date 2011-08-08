using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RecommenderSystem.Knn
{
    public static class Data
    {
        public static IEnumerable<User> LoadData(string filename, int limit = 0)
        {
            TextReader reader = new StreamReader(filename);

            string line;
            string userId = String.Empty;
            List<string[]> plays = new List<string[]>();

            int count = 0;
            while ((line = reader.ReadLine()) != null && (count < limit || limit == 0))
            {
                var parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (parts[0] == userId)
                {
                    plays.Add(parts);
                }
                else
                {
                    if (plays.Count > 0)
                    {
                        yield return User.CreateFormPlays(plays);
                        count++;
                    }

                    userId = parts[0];
                    plays = new List<string[]>();
                    plays.Add(parts);
                }
            }
        }
    }
}
