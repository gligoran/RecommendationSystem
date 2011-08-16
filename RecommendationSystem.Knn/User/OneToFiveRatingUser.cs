using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommendationSystem.Knn
{
    public class OneToFiveRatingUser : User
    {
        #region Constructor
        public OneToFiveRatingUser(string userId, List<string[]> data)
            : base(userId, data)
        { }
        #endregion

        #region
        protected override void PreprocessRatings()
        {
            var keys = (from k in Ratings.Keys
                        orderby Ratings[k] descending
                        select k).ToList();

            Ratings = new Dictionary<string, float>();
            for (int i = 0; i < keys.Count; i++)
                Ratings.Add(keys[i], 5 - i * 5 / keys.Count);

            TotalPlays = 1;
            AverageRating = 2.5f;
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
            u += string.Format("== {0}{1}", Ratings.Count, Environment.NewLine);

            return u;
        }
        #endregion
    }
}
