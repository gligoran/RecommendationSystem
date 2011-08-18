using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Similarity;

namespace RecommendationSystem.Knn
{
    public static class Manager
    {
        #region LoadData
        public static IEnumerable<T> LoadData<T>(string filename, int limit = int.MaxValue, bool covertToRatings = false)
            where T : User.User
        {
            TextReader reader = new StreamReader(filename);

            string line;
            var userId = String.Empty;
            var lines = new List<string[]>();

            var count = limit;
            var sep = new[] { "\t" };
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
                    lines = new List<string[]> { parts };
                }
            }

            //add last user if unlimited
            if (limit == int.MaxValue)
                yield return (T)Activator.CreateInstance(typeof(T), new object[] { userId, lines });

            reader.Close();
        }
        #endregion

        #region CalculateNeighbours
        public static void CalculateKNearestNeighbours(List<User.User> users, ISimilarityEstimator similarityEstimator, int k = 3)
        {
            for (var i = 0; i < users.Count; i++)
            {
                CalculateKNearestNeighboursForUser(users[i], users, similarityEstimator, i + 1);
            }
        }

        public static void CalculateKNearestNeighboursForUser(User.User user, List<User.User> users, ISimilarityEstimator similarityEstimator, int offset = 0, int k = 3)
        {
            for (var i = offset; i < users.Count; i++)
            {
                if (user == users[i])
                    continue;

                var s = similarityEstimator.Similarity(user, users[i]);

                if (s <= 0.0) continue;

                user.Neighbours.Add(new SimilarityEstimate(users[i], s));
                users[i].Neighbours.Add(new SimilarityEstimate(user, s));

                PruneNeighbours(user, k);
                PruneNeighbours(users[i], k);
            }
        }

        public static void PruneNeighbours(User.User user, int k = 3)
        {
            user.Neighbours.Sort();

            while (user.Neighbours.Count > k)
                user.Neighbours.RemoveAt(user.Neighbours.Count - 1);
        }
        #endregion

        #region GetRecommendations
        public static List<Recommendation> GetRecommendations(User.User user, IRatingAggregator ratingAggregator, int n = 5)
        {
            if (user.Neighbours.Count == 0)
                return null;

            var recommendations = new List<Recommendation>();

            var artists = new List<string>();
            artists = user.Neighbours.Aggregate(artists, (current, neighbour) => current.Union(neighbour.SimilarUser.Ratings.Keys).ToList());

            artists = artists.Except(user.Ratings.Keys).ToList();

            foreach (var artist in artists)
            {
                float r = ratingAggregator.Aggregate(user, artist);
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
