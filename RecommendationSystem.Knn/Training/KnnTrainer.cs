using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Training;

namespace RecommendationSystem.Knn.Training
{
    public class KnnTrainer : ITrainer<IKnnModel, IUser>
    {
        public IKnnModel TrainModel(List<IUser> users, List<IArtist> artists, List<IRating> ratings)
        {
            var model = new KnnModel();
            foreach (var user in users)
                model.Users.Add(KnnUser.FromIUser(user));

            return model;
        }
    }
}