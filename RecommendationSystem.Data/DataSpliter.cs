using System;
using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Data
{
    public static class DataSpliter
    {
        public static void SplitIntoTrainAndTest(this List<IUser> users, out List<IUser> train, out List<IUser> test, float trainShare = 0.7f)
        {
            train = new List<IUser>();

            var indices = users.Select((t, i) => i).ToList();

            var random = new Random();
            var count = (int)(users.Count * trainShare);
            for (var i = 0; i < count; i++)
            {
                var index = indices[random.Next(indices.Count)];
                train.Add(users[index]);

                indices.Remove(index);
            }

            test = indices.Select(index => new User(users[index].UserId, users[index].SignUp, users[index].Gender, users[index].Age, users[index].Country)).Cast<IUser>().ToList();
        }
    }
}