using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Training
{
    public interface ISvdBoostedKnnTrainer<TSvdBoostedKnnModel> : ISvdTrainer<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        INewUserFeatureGenerator<TSvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        TSvdBoostedKnnModel TrainSvdBoostedKnnModelFromSvdModel(string filename, List<IUser> trainUsers);
        TSvdBoostedKnnModel TrainSvdBoostedKnnModelFromSvdModel(TSvdBoostedKnnModel model, List<IUser> trainUsers);
        TSvdBoostedKnnModel LoadSvdModel(string filename);
    }

    public abstract class SvdBoostedKnnTrainerBase<TSvdBoostedKnnModel> : SvdTrainerBase<TSvdBoostedKnnModel>, ISvdBoostedKnnTrainer<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        public INewUserFeatureGenerator<TSvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        protected ModelLoader<TSvdBoostedKnnModel> ModelLoader { get; set; }

        protected SvdBoostedKnnTrainerBase(INewUserFeatureGenerator<TSvdBoostedKnnModel> newUserFeatureGenerator)
        {
            NewUserFeatureGenerator = newUserFeatureGenerator;

            ModelLoader = new ModelLoader<TSvdBoostedKnnModel>();
            ModelLoader.ModelPartLoaders.Add(new SvdModelPartLoader());
        }

        public new TSvdBoostedKnnModel TrainModel(List<IUser> trainUsers, List<IArtist> artists, List<IRating> trainRatings)
        {
            var model = base.TrainModel(trainUsers, artists, trainRatings);
            return TrainSvdBoostedKnnModelFromSvdModel(model, trainUsers);
        }

        public TSvdBoostedKnnModel TrainSvdBoostedKnnModelFromSvdModel(string filename, List<IUser> trainUsers)
        {
            var model = LoadSvdModel(filename);
            return TrainSvdBoostedKnnModelFromSvdModel(model, trainUsers);
        }

        public TSvdBoostedKnnModel TrainSvdBoostedKnnModelFromSvdModel(TSvdBoostedKnnModel model, List<IUser> trainUsers)
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
    }
}