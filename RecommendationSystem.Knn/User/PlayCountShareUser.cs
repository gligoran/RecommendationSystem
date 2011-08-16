using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace RecommendationSystem.Knn
{
    public class PlayCountShareUser : User
    {
        #region Constructor
        public PlayCountShareUser(string userId, List<string[]> data)
            : base(userId, data)
        { }
        #endregion

        #region PreprocessRatings
        protected override void PreprocessRatings()
        {
            var oldRatings = Ratings;
            Ratings = new Dictionary<string, float>();
            foreach (var artist in oldRatings.Keys)
                Ratings.Add(artist, oldRatings[artist] / (float)TotalPlays);

            AverageRating = 1.0f / Ratings.Count;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            string u = UserId + Environment.NewLine;
            foreach (var play in Ratings)
            {
                u += string.Format("- {0} [{1}]{2}", play.Key, play.Value * TotalPlays, Environment.NewLine);
            }
            u += string.Format("== {0} [{1}]{2}", Ratings.Count, TotalPlays, Environment.NewLine);

            return u;
        }
        #endregion
    }
}
