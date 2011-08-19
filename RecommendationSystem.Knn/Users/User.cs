using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Knn.Similarity;

namespace RecommendationSystem.Knn.Users
{
    public class User
    {
        #region Properties
        public string UserId { get; set; }
        public Dictionary<string, float> Ratings { get; set; }
        public int TotalPlays { get; set; }
        public List<SimilarityEstimate> Neighbours { get; set; }
        public float AverageRating { get; set; }
        #endregion

        #region Consturctor
        public User(string userId)
        {
            UserId = userId;
            TotalPlays = 0;
            Ratings = new Dictionary<string, float>();
            Neighbours = new List<SimilarityEstimate>();
        }

        /* Line is in format:
         * <user, mbid, artist, playcount>
        */
        public User(string userId, IEnumerable<string[]> lines)
            : this(userId)
        {
            foreach (var line in lines)
            {
                var count = int.Parse(line[3]);
                if (Ratings.ContainsKey(line[2]))
                    Ratings[line[2]] += count;
                else
                    Ratings.Add(line[2], count);

                TotalPlays += count;
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            var u = UserId + Environment.NewLine;
            u = Ratings.Aggregate(u, (c, p) => c + string.Format("- {0} [{1}]{2}", p.Key, p.Value, Environment.NewLine));
            u += string.Format("== {0} [{1}]{2}", Ratings.Count, TotalPlays, Environment.NewLine);

            return u;
        }
        #endregion

        #region Virtual Methods
        protected virtual void PreprocessRatings() { }
        #endregion

    }
}
