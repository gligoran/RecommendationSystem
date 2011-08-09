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
            double rX, rY;
            double sumNum = 0.0, sumX = 0.0, sumY = 0.0;

            int count = 0;
            foreach (var artist in this.Ratings.Keys)
            {
                if (other.Ratings.Keys.Contains(artist))
                {
                    count++;

                    rX = (double)this.Ratings[artist] / this.TotalPlays;
                    rY = (double)other.Ratings[artist] / other.TotalPlays;
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

                    rXavg += (double)this.Ratings[artist] / this.TotalPlays;
                    rYavg += (double)other.Ratings[artist] / other.TotalPlays;
                }
            }
            rXavg /= keys.Count;
            rYavg /= keys.Count;

            foreach (var artist in keys)
            {
                rX = (double)this.Ratings[artist] / this.TotalPlays - rXavg;
                rY = (double)other.Ratings[artist] / other.TotalPlays - rYavg;

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
    }
}
