using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.SimpleKnn.Users
{
    public class SimpleKnnUser : User, ISimpleKnnUser
    {
        public float AverageRating { get; set; }
        public Dictionary<int, IRating> RatingsByArtistIndexLookupTable { get; set; }
        public List<int> ArtistIndices { get; set; }

        public SimpleKnnUser(string userId, List<IRating> ratings, Dictionary<int, IRating> artistIndexRatings, DateTime signUp, string gender = "", int age = -1, string country = "")
            : base(userId, signUp, gender, age, country)
        {
            Ratings = ratings;
            RatingsByArtistIndexLookupTable = artistIndexRatings;

            ArtistIndices = new List<int>(artistIndexRatings.Keys);
            ArtistIndices.Sort();

            AverageRating = Ratings.Count > 0 ? Ratings.Average(rating => rating.Value) : 0.0f;
        }

        public static ISimpleKnnUser FromIUser(IUser user)
        {
            var a = user.Ratings.ToDictionary(rating => rating.ArtistIndex);
            return new SimpleKnnUser(user.UserId, user.Ratings, a, user.SignUp, user.Gender, user.Age, user.Country);
        }

#if DEBUG
        public override string ToString()
        {
            var str = String.Format("AVG: {0}{3}AIRcount: {1}{3}AIcount:{2}{3}", AverageRating, RatingsByArtistIndexLookupTable.Count, ArtistIndices.Count, Environment.NewLine);
            return str + base.ToString();
        }
#endif
    }
}