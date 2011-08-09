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
            double rX, rY;
            double sumNum = 0.0, sumX = 0.0, sumY = 0.0;

            int count = 0;
            foreach (var artist in this.Ratings.Keys)
            {
                if (other.Ratings.Keys.Contains(artist))
                {
                    count++;

                    rX = (double)this.Ratings[artist];
                    rY = (double)other.Ratings[artist];
                    sumNum += rX * rY;
                    sumX += Math.Pow(rX, 2);
                    sumY += Math.Pow(rY, 2);
                }
            }

            if (count == 0)
                return 0;

            double mass = count * 2.0 / (this.Ratings.Count + other.Ratings.Count);
            return sumNum / (Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
        }
        #endregion

        #region PearsonSimliarity
        public override double PearsonSimliarity(User other)
        {
            double rX, rY;
            double rXavg = 0.0, rYavg = 0.0;
            double sumNum = 0.0, sumX = 0.0, sumY = 0.0;

            var keys = new List<string>();
            foreach (var artist in this.Ratings.Keys)
            {
                if (other.Ratings.Keys.Contains(artist))
                {
                    keys.Add(artist);

                    rXavg += this.Ratings[artist] ;
                    rYavg += other.Ratings[artist] ;
                }
            }
            rXavg /= keys.Count;
            rYavg /= keys.Count;

            foreach (var artist in keys)
            {
                rX = this.Ratings[artist] - rXavg;
                rY = other.Ratings[artist] - rYavg;

                sumNum += rX * rY;
                sumX += Math.Pow(rX, 2);
                sumY += Math.Pow(rY, 2);
            }

            if (keys.Count == 0)
                return 0;

            double mass = keys.Count * 2.0 / (this.Ratings.Count + other.Ratings.Count);
            return sumNum / (Math.Sqrt(sumX) * Math.Sqrt(sumY)) * (mass);
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
