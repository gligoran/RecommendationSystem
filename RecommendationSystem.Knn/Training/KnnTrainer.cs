using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Training;

namespace RecommendationSystem.Knn.Training
{
    public class KnnTrainer : ITrainer<IKnnModel>
    {
        public List<IUser> Users { get; set; }

        public KnnTrainer(List<IUser> users)
        {
            Users = users;
        }

        public IKnnModel TrainModel()
        {
            var model = new KnnModel();
            foreach (var user in Users)
                model.Users.Add(KnnUser.FromIUser(user));

            return model;
        }
    }
}