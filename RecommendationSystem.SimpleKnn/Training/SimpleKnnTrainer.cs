using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Users;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleKnn.Training
{
    public class SimpleKnnTrainer : ITrainer<ISimpleKnnModel>
    {
        public ISimpleKnnModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings)
        {
            var model = new SimpleKnnModel();
            foreach (var user in trainUsers)
                model.Users.Add(SimpleKnnUser.FromIUser(user));

            return model;
        }
    }
}