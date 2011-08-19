using System.Collections.Generic;
using System;
using System.Linq;

namespace RecommendationSystem.Knn.Users
{
    public sealed class PlayCountShareUser : User
    {
        #region Constructor
        public PlayCountShareUser(string userId, IEnumerable<string[]> data)
            : base(userId, data)
        {
            PreprocessRatings();
        }
        #endregion

        #region PreprocessRatings
        protected override void PreprocessRatings()
        {
            var oldRatings = Ratings;
            Ratings = new Dictionary<string, float>();
            foreach (var artist in oldRatings.Keys)
                Ratings.Add(artist, oldRatings[artist] / TotalPlays);

            AverageRating = 1.0f / Ratings.Count;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            var u = UserId + Environment.NewLine;
            u = Ratings.Aggregate(u, (c, p) => c + string.Format("- {0} [{1}]{2}", p.Key, p.Value * TotalPlays, Environment.NewLine));
            u += string.Format("== {0} [{1}]{2}", Ratings.Count, TotalPlays, Environment.NewLine);

            return u;
        }
        #endregion
    }
}
