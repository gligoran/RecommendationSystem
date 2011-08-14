using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using RecommendationSystem.Knn.Similarity;

namespace RecommendationSystem.Knn
{
    public class User
    {
        #region Properties
        public string UserId { get; set; }
        public Dictionary<string, double> Ratings { get; set; }
        public int TotalPlays { get; set; }
        public List<SimilarityEstimate> Neighbours { get; set; }
        #endregion

        #region Consturctor
        public User(string userId)
        {
            this.UserId = userId;
            this.TotalPlays = 0;
            this.Ratings = new Dictionary<string, double>();
            this.Neighbours = new List<SimilarityEstimate>();
        }

        public User(List<string[]> lines)
            : this(lines[0][0])
        {
            Contract.Requires(lines.Count > 0);

            int count;
            foreach (var line in lines)
            {
                count = int.Parse(line[2]);
                if (this.Ratings.ContainsKey(line[1]))
                    this.Ratings[line[1]] += count;
                else
                    this.Ratings.Add(line[1], count);

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

        public double AverageRating { get; set; }
    }
}
