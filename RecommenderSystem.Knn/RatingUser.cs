using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Knn
{
    public class RatingUser : User
    {
        #region Constructor
        public RatingUser(string userId)
            : base(userId)
        { }

        public RatingUser(List<string[]> data)
            : base(data)
        {
            ConvertToRatings();
        }
        #endregion

        #region CosineSimliarity
        public override double CosineSimliarity(User other)
        {
            double r_x, r_y;
            double sum_num = 0.0f, sum_x = 0.0f, sum_y = 0.0f;

            int count = 0;
            foreach (var artist in this.Ratings.Keys)
            {
                if (other.Ratings.Keys.Contains(artist))
                {
                    count++;

                    r_x = (double)this.Ratings[artist];
                    r_y = (double)other.Ratings[artist];
                    sum_num += r_x * r_y;
                    sum_x += r_x * r_x;
                    sum_y += r_y * r_y;
                }
            }

            if (count == 0)
                return 0;

            double mass = count * 2.0 / (this.Ratings.Count + other.Ratings.Count);
            return sum_num / (Math.Sqrt(sum_x) * Math.Sqrt(sum_y)) * (mass);
        }
        #endregion

        #region Helpers

        #region SortRatings
        private void SortRatings()
        {
            var sortedKeys = from k in Ratings.Keys
                             orderby Ratings[k] descending
                             select k;

            var sortedRatings = new Dictionary<string, int>();
            foreach (var key in sortedKeys)
            {
                sortedRatings.Add(key, Ratings[key]);
            }

            Ratings = sortedRatings;
        }
        #endregion

        #region ConvertToRatings
        private void ConvertToRatings()
        {
            SortRatings();
            var keys = Ratings.Keys.ToList();
            int count = keys.Count;
            for (int i = 0; i < count; i++)
            {
                Ratings[keys[i]] = 5 - i * 5 / count;
            }
        }
        #endregion

        #endregion
    }
}
