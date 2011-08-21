using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Knn.Users
{
    public class KnnUser : User, IKnnUser
    {
        public float AverageRating { get; set; }

        public KnnUser(string userId, List<IRating> ratings, DateTime signUp, string gender = "", int age = -1, string country = "")
            : base(userId, signUp, gender, age, country)
        {
            Ratings = ratings;
            AverageRating = Ratings.Average(rating => rating.Value);
        }

        public static IKnnUser FromIUser(IUser user)
        {
            return new KnnUser(user.UserId, user.Ratings, user.SignUp, user.Gender, user.Age, user.Country);
        }
    }
}