using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Knn
{
    public class OneToFiveRatingUser : User
    {
        #region Constructor
        public OneToFiveRatingUser(List<string[]> data)
            : base(data)
        { }
        #endregion

        #region
        protected override void PreprocessRatings()
        {
            var keys = (from k in Ratings.Keys
                        orderby Ratings[k] descending
                        select k).ToList();

            Ratings = new Dictionary<string, double>();
            for (int i = 0; i < keys.Count; i++)
            {
                Ratings.Add(keys[i], 5 - i * 5 / keys.Count);
            }
        }
        #endregion
    }
}
