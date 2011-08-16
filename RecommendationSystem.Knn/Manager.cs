using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Recommendations;

namespace RecommendationSystem.Knn
{
    public static class Manager
    {
        #region LoadData
        public static IEnumerable<T> LoadData<T>(string filename, int limit = int.MaxValue, bool covertToRatings = false)
            where T : User
        {
            TextReader reader = new StreamReader(filename);

            string line;
            string userId = String.Empty;
            List<string[]> lines = new List<string[]>();

            int count = limit;
            string[] sep = new string[] { "\t" };
            while ((line = reader.ReadLine()) != null && count > 0)
            {
                var parts = line.Split(sep, StringSplitOptions.None);
                if (parts[0] == userId)
                {
                    lines.Add(parts);
                }
                else
                {
                    if (lines.Count > 0)
                    {
                        yield return (T)Activator.CreateInstance(typeof(T), new object[] { userId, lines });
                        count--;
                    }

                    userId = parts[0];
                    lines = new List<string[]>();
                    lines.Add(parts);
                }
            }

            //add last user if unlimited
            if (limit == int.MaxValue)
                yield return (T)Activator.CreateInstance(typeof(T), new object[] { userId, lines });
            
            reader.Close();
        }
        #endregion

        #region CalculateNeighbours
        public static void CalculateKNearestNeighbours(List<User> users, ISimilarityEstimator similarityEstimator, int k = 3)
        {
            for (int i = 0; i < users.Count; i++)
            {
                CalculateKNearestNeighboursForUser(users[i], users, similarityEstimator, offset: i + 1);
            }
        }

        public static void CalculateKNearestNeighboursForUser(User user, List<User> users, ISimilarityEstimator similarityEstimator, int offset = 0, int k = 3)
        {
            float s;
            for (int i = offset; i < users.Count; i++)
            {
                if (user == users[i])
                    continue;

                s = similarityEstimator.Similarity(user, users[i]);

                if (s > 0.0)
                {
                    user.Neighbours.Add(new SimilarityEstimate(users[i], s));
                    users[i].Neighbours.Add(new SimilarityEstimate(user, s));

                    PruneNeighbours(user, k);
                    PruneNeighbours(users[i], k);
                }
            }
        }

        public static void PruneNeighbours(User user, int k = 3)
        {
            user.Neighbours.Sort();

            while (user.Neighbours.Count > k)
                user.Neighbours.RemoveAt(user.Neighbours.Count - 1);
        }
        #endregion

        #region GetRecommendations
        public static List<Recommendation> GetRecommendations(User user, IRatingAggregator ratingAggregator, int n = 5)
        {
            if (user.Neighbours.Count == 0)
                return null;

            var recommendations = new List<Recommendation>();

            var artists = new List<string>();
            foreach (var neighbour in user.Neighbours)
                artists = artists.Union(neighbour.SimilarUser.Ratings.Keys).ToList();

            artists = artists.Except(user.Ratings.Keys).ToList<string>();

            float r;
            foreach (var artist in artists)
            {
                r = ratingAggregator.Aggregate(user, artist);
                if (r > 0.0f)
                {
                    recommendations.Add(new Recommendation(artist, r));
                    PruneRecommendations(recommendations, n);
                }
            }

            return recommendations;
        }

        private static void PruneRecommendations(List<Recommendation> recommendations, int n = 5)
        {
            recommendations.Sort();
            while (recommendations.Count > n)
                recommendations.RemoveAt(recommendations.Count - 1);
        }
        #endregion
    }
}
