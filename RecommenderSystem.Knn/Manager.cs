using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RecommenderSystem.Knn.Similarity;

namespace RecommenderSystem.Knn
{
    public static class Manager
    {
        #region LoadData
        public static IEnumerable<T> LoadData<T>(string filename, int limit = 0, bool covertToRatings = false)
            where T : User
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
        #endregion

        #region CalculateNeighbours
        public static void CalculateKNearestNeighbours(List<User> users, ISimilarityEstimator similarityEstimator, int k = 3)
        {
            double s;

            for (int i = 0; i < users.Count; i++)
            {
                for (int j = i + 1; j < users.Count; j++)
                {
                    s = similarityEstimator.Similarity(users[i], users[j]);

                    if (s > 0.0)
                    {
                        users[i].Neighbours.Add(new SimilarityEstimate(users[j], s));
                        users[j].Neighbours.Add(new SimilarityEstimate(users[i], s));

                        PruneNeighbours(users[i], k);
                        PruneNeighbours(users[j], k);
                    }
                }
            }
        }

        public static void PruneNeighbours(User user, int k = 3)
        {
            while (user.Neighbours.Count > k)
                user.Neighbours.Remove(user.Neighbours.Min);
        }
        #endregion
    }
}
