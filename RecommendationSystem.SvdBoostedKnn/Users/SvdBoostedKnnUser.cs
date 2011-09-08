using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.SvdBoostedKnn.Users
{
    public class SvdBoostedKnnUser : KnnUser, ISvdBoostedKnnUser
    {
        public float[] Features { get; set; }

        public SvdBoostedKnnUser(string userId, float[] features, List<IRating> ratings, DateTime signUp, string gender = "", int age = -1, string country = "")
            : base(userId, ratings, signUp, gender, age, country)
        {
            Features = features;
        }

        public static ISvdBoostedKnnUser FromIUser(IUser user, float[] features)
        {
            return new SvdBoostedKnnUser(user.UserId, features, user.Ratings, user.SignUp, user.Gender, user.Age, user.Country);
        }
    }
}