using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace RecommenderSystem.Knn
{
    public abstract class User
    {
        #region Properties
        public string UserId { get; set; }
        public Dictionary<string, int> Ratings { get; set; }
        public int TotalPlays { get; set; }
        public SortedSet<User> Neighbours { get; set; }
        #endregion

        #region Consturctor
        public User(string userId)
        {
            this.UserId = userId;
            this.TotalPlays = 0;
            this.Ratings = new Dictionary<string, int>();
            this.Neighbours = new SortedSet<User>();
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

        #region Abstract Methods
        public abstract double CosineSimliarity(User other);
        public abstract double PearsonSimliarity(User other);
        #endregion
    }
}
