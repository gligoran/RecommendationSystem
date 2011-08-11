using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace RecommenderSystem.Knn
{
    public class PlayCountShareUser : User
    {
        #region Constructor
        public PlayCountShareUser(List<string[]> data)
            : base(data)
        { }
        #endregion

        #region RatingPreprocessing
        protected override void PreprocessRatings()
        {
            foreach (var rating in Ratings.Keys)
                Ratings[rating] /= TotalPlays;
        }
        #endregion
    }
}
