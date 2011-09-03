using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.SimpleKnn.Users
{
    public class SimpleKnnUser : KnnUser, ISimpleKnnUser
    {
        public Dictionary<int, IRating> RatingsByArtistIndexLookupTable { get; set; }
        public List<int> ArtistIndices { get; set; }

        public SimpleKnnUser(string userId, List<IRating> ratings, Dictionary<int, IRating> artistIndexRatings, DateTime signUp, string gender = "", int age = -1, string country = "")
            : base(userId, ratings, signUp, gender, age, country)
        {
            RatingsByArtistIndexLookupTable = artistIndexRatings;

            ArtistIndices = new List<int>(artistIndexRatings.Keys);
            ArtistIndices.Sort();
        }

        public static new ISimpleKnnUser FromIUser(IUser user)
        {
            var a = user.Ratings.ToDictionary(rating => rating.ArtistIndex);
            return new SimpleKnnUser(user.UserId, user.Ratings, a, user.SignUp, user.Gender, user.Age, user.Country);
        }
    }
}