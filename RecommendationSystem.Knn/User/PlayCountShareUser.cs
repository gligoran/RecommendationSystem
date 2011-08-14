using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace RecommendationSystem.Knn
{
    public class PlayCountShareUser : User
    {
        #region Constructor
        public PlayCountShareUser(List<string[]> data)
            : base(data)
        { }
        #endregion

        #region PreprocessRatings
        protected override void PreprocessRatings()
        {
            var oldRatings = Ratings;
            Ratings = new Dictionary<string, double>();
            foreach (var artist in oldRatings.Keys)
                Ratings.Add(artist, oldRatings[artist] / (double)TotalPlays);

            AverageRating = 1.0 / Ratings.Count;
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
