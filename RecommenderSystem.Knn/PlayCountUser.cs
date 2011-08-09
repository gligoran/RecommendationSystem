using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace RecommenderSystem.Knn
{
    public class PlayCountUser : User
    {
        #region Constructor
        public PlayCountUser(string userId)
            : base(userId)
        { }

        public PlayCountUser(List<string[]> data)
            : base(data)
        { }
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

                    r_x = (double)this.Ratings[artist] / (double)this.TotalPlays;
                    r_y = (double)other.Ratings[artist] / (double)other.TotalPlays;
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
    }
}
