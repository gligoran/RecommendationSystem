using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public abstract class KnnTrainerForSvdModelsBase<TSvdBoostedKnnModel> : IKnnTrainerForSvdModels<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        public INewUserFeatureGenerator<TSvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        protected ModelLoader<TSvdBoostedKnnModel> ModelLoader { get; set; }

        protected KnnTrainerForSvdModelsBase(INewUserFeatureGenerator<TSvdBoostedKnnModel> newUserFeatureGenerator)
        {
            NewUserFeatureGenerator = newUserFeatureGenerator;

            ModelLoader = new ModelLoader<TSvdBoostedKnnModel>();
            ModelLoader.ModelPartLoaders.Add(new SvdModelPartLoader());
        }

        public TSvdBoostedKnnModel TrainKnnModel(string filename, List<IUser> trainUsers)
        {
            var model = LoadSvdModel(filename);
            return TrainKnnModel(model, trainUsers);
        }

        public TSvdBoostedKnnModel TrainKnnModel(TSvdBoostedKnnModel model, List<IUser> trainUsers)
        {
            foreach (var user in trainUsers)
                model.Users.Add(SvdBoostedKnnUser.FromIUser(user, NewUserFeatureGenerator.GetNewUserFeatures(model, user)));

            return model;
        }

        public TSvdBoostedKnnModel LoadSvdModel(string filename)
        {
            var model = GetNewModelInstance();
            ModelLoader.LoadModel(model, filename);
            return model;
        }

        protected abstract TSvdBoostedKnnModel GetNewModelInstance();
    }
}