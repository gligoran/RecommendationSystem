using System;
using System.Collections.Generic;
using System.Linq;

namespace RecommendationSystem.Knn.Users
{
    public sealed class OneToFiveRatingUser : User
    {
        #region Constructor
        public OneToFiveRatingUser(string userId, IEnumerable<string[]> data)
            : base(userId, data)
        {
            PreprocessRatings();
        }
        #endregion

        #region
        protected override void PreprocessRatings()
        {
            var keys = (from k in Ratings.Keys
                        orderby Ratings[k] descending
                        select k).ToList();

            Ratings = new Dictionary<string, float>();
            for (var i = 0; i < keys.Count; i++)
                Ratings.Add(keys[i], 5 - i * 5 / keys.Count);

            TotalPlays = 1;
            AverageRating = 2.5f;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            var u = UserId + Environment.NewLine;
            u = Ratings.Aggregate(u, (c, r) => c + string.Format("- {0} [{1}]{2}", r.Key, r.Value, Environment.NewLine));
            u += string.Format("== {0}{1}", Ratings.Count, Environment.NewLine);

            return u;
        }
        #endregion
    }
}