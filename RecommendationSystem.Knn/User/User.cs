using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecommendationSystem.Knn.Similarity;

namespace RecommendationSystem.Knn
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
            this.UserId = userId;
            this.TotalPlays = 0;
            this.Ratings = new Dictionary<string, float>();
            this.Neighbours = new List<SimilarityEstimate>();
        }

        /* Line is in format:
         * <user, mbid, artist, playcount>
        */
        public User(string userId, List<string[]> lines)
            : this(userId)
        {
            int count;
            foreach (var line in lines)
            {
                count = int.Parse(line[3]);
                if (this.Ratings.ContainsKey(line[2]))
                    this.Ratings[line[2]] += count;
                else
                    this.Ratings.Add(line[2], count);

                this.TotalPlays += count;
            }

            PreprocessRatings();
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            string u = UserId + Environment.NewLine;
            foreach (var play in Ratings)
            {
                u += string.Format("- {0} [{1}]{2}", play.Key, play.Value, Environment.NewLine);
            }
            u += string.Format("== {0} [{1}]{2}", Ratings.Count, TotalPlays, Environment.NewLine);

            return u;
        }
        #endregion

        #region Virtual Methods
        protected virtual void PreprocessRatings() { }
        #endregion

    }
}
