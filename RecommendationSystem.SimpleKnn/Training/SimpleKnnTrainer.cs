using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Users;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleKnn.Training
{
    public class SimpleKnnTrainer : ITrainer<ISimpleKnnModel, IUser>
    {
        public ISimpleKnnModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            var model = new SimpleKnnModel();
            foreach (var user in users)
                model.Users.Add(SimpleKnnUser.FromIUser(user));

            return model;
        }
    }
}