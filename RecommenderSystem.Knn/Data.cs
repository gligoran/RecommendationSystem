using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RecommenderSystem.Knn
{
    public static class Data
    {
        public static IEnumerable<T> LoadData<T>(string filename, int limit = 0, bool covertToRatings = false) where T : User
        {
            TextReader reader = new StreamReader(filename);

            string line;
            string userId = String.Empty;
            List<string[]> lines = new List<string[]>();

            int count = 0;
            while ((line = reader.ReadLine()) != null && (count < limit || limit == 0))
            {
                var parts = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                if (parts[0] == userId)
                {
                    lines.Add(parts);
                }
                else
                {
                    if (lines.Count > 0)
                    {
                        yield return (T)Activator.CreateInstance(typeof(T), new object[] { lines });
                        count++;
                    }

                    userId = parts[0];
                    lines = new List<string[]>();
                    lines.Add(parts);
                }
            }
        }
    }
}
